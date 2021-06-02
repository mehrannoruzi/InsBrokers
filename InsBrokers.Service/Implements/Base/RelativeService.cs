using System;
using Elk.Core;
using System.IO;
using System.Linq;
using System.Text;
using InsBrokers.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InsBrokers.CrossCutting;
using InsBrokers.DataAccess.Ef;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using InsBrokers.Service.Resource;
using DomainStrings = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Service
{
    public class RelativeService : IRelativeService
    {
        private readonly AppUnitOfWork _appUow;
        private readonly IGenericRepo<Relative> _RelativeRepo;

        public RelativeService(AppUnitOfWork appUOW)
        {
            _appUow = appUOW;
            _RelativeRepo = appUOW.RelativeRepo;
        }

        public PagingListDetails<Relative> Get(RelativeSearchFilter filter)
        {
            Expression<Func<Relative, bool>> conditions = x => true;
            if (filter != null)
            {
                if (filter.UserId != null)
                    conditions = conditions.And(x => x.UserId == filter.UserId);
                if (!string.IsNullOrWhiteSpace(filter.NationalCode))
                    conditions = conditions.And(x => x.NationalCode == filter.NationalCode);
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    conditions = conditions.And(x => (x.Name + " " + x.Family).Contains(filter.Name));
            }

            return _RelativeRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.RelativeId)); ;
        }


        public async Task<IResponse<Relative>> AddAsync(Relative model)
        {
            using var bt = _appUow.Database.BeginTransaction();
            await _appUow.RelativeRepo.AddAsync(model);
            var addRelativeResult = await _appUow.ElkSaveChangesAsync();
            if (!addRelativeResult.IsSuccessful)
                return new Response<Relative> { Message = addRelativeResult.Message };
            #region Save Attachments
            if (model.RelativeAttachmentIds != null && model.RelativeAttachmentIds.Count > 0)
            {
                var attachments = _appUow.RelativeAttachmentRepo.Get(conditions: x => model.RelativeAttachmentIds.Contains(x.RelativeAttachmentId), orderBy: o => o.OrderBy(x => x.RelativeAttachmentId));
                if (attachments != null)
                    foreach (var attachment in attachments)
                    {
                        attachment.RelativeId = model.RelativeId;
                        _appUow.RelativeAttachmentRepo.Update(attachment);
                    }
                var updateAttchResult = await _appUow.ElkSaveChangesAsync();
                if (!updateAttchResult.IsSuccessful)
                {
                    bt.Rollback();
                    return new Response<Relative> { Message = updateAttchResult.Message };
                }
            }
            #endregion
            bt.Commit();
            return new Response<Relative> { IsSuccessful = true };
        }

        public async Task<IResponse<Relative>> UpdateAsync(Relative model)
        {
            var Relative = await _RelativeRepo.FindAsync(model.RelativeId);
            if (Relative == null) return new Response<Relative> { Message = ServiceMessage.RecordNotExist };

            Relative.Name = model.Name;
            Relative.Family = model.Family;
            Relative.FatherName = model.FatherName;
            Relative.Gender = model.Gender;
            Relative.BirthDay = model.BirthDay;
            Relative.BirthDayMi = PersianDateTime.Parse(model.BirthDay).ToDateTime();
            Relative.IdentityNumber = model.IdentityNumber;
            Relative.NationalCode = model.NationalCode;
            Relative.RelativeType = model.RelativeType;
            Relative.TakafolKind = model.TakafolKind;
            _RelativeRepo.Update(Relative);
            #region Save Attachments
            if (model.RelativeAttachmentIds != null && model.RelativeAttachmentIds.Count > 0)
            {
                var attachments = _appUow.RelativeAttachmentRepo.Get(conditions: x => model.RelativeAttachmentIds.Contains(x.RelativeAttachmentId), orderBy: o => o.OrderBy(ClosedXML => ClosedXML.InsertDateMi));
                if (attachments != null)
                    foreach (var attachment in attachments)
                    {
                        attachment.RelativeId = model.RelativeId;
                        _appUow.RelativeAttachmentRepo.Update(attachment);
                    }
            }
            #endregion
            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<Relative> {  IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            _RelativeRepo.Delete(new Relative { RelativeId = id });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<Relative>> FindAsync(int id)
        {
            var relative = await _RelativeRepo.FirstOrDefaultAsync(x => x.RelativeId == id,
                new List<Expression<Func<Relative, object>>> { x => x.RelativeAttachments });
            if (relative == null) return new Response<Relative> { Message = ServiceMessage.RecordNotExist };
            if (relative.RelativeAttachments != null)
                relative.RelativeAttachments = relative.RelativeAttachments.Where(x => !x.IsDeleted).ToList();
            return new Response<Relative> { Result = relative, IsSuccessful = true };
        }

        public async Task<IResponse<Relative>> FindWithAttachmentsAsync(int id)
        {
            var relative = await _appUow.RelativeRepo.FirstOrDefaultAsync(
                conditions: x => x.RelativeId == id,
                includeProperties: new List<Expression<Func<Relative, object>>>
                {
                    x => x.RelativeAttachments,
                    x=> x.User
                });
            if (relative.RelativeAttachments != null)
                relative.RelativeAttachments = relative.RelativeAttachments.Where(x => !x.IsDeleted).ToList();
            if (relative == null) return new Response<Relative> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Relatives) };

            return new Response<Relative> { Result = relative, IsSuccessful = true };
        }

        public async Task<IResponse<object>> AddAttachments(Guid userId, IFormFile file, AttachmentType type)
        {
            var response = new Response<object>();
            try
            {
                if (file.IsNull() || file.Length < 0) return new Response<object> { Message = "هیچ فایلی آپلود نشده است." };
                if (file.Length > (10000 * 1024)) return new Response<object> { Message = "فایل آپلود شده نباید بیشتر از 10MB باشد." };

                #region Save Attachment To Host
                var fullPath = HttpFileTools.GetPath(
                    fileNameWithExtension: type.ToString() + Path.GetExtension(file.FileName),
                    root: $"Files/RelativeAttachment/{userId}");
                if (string.IsNullOrWhiteSpace(fullPath)) return new Response<object> { Message = ServiceMessage.Error };

                var fileUrl = HttpFileTools.Save(file, fullPath);
                #endregion

                #region Save Attachment To Database
                var relativeAttachment = new RelativeAttachment
                {
                    Url = fileUrl,
                    Size = file.Length,
                    UserAttachmentType = type,
                    FileType = FileType.Image,
                    Extention = Path.GetExtension(file.FileName),
                    Name = Path.GetFileNameWithoutExtension(fileUrl),
                    RelativeId = 1
                };

                await _appUow.RelativeAttachmentRepo.AddAsync(relativeAttachment);
                var fileSaveResult = await _appUow.ElkSaveChangesAsync();
                if (fileSaveResult.IsSuccessful)
                {
                    response.IsSuccessful = true;
                    response.Result = relativeAttachment;
                    response.Message = ServiceMessage.Success;
                }
                else
                    response.Message = ServiceMessage.Error;
                #endregion

                return response;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                response.Message = ServiceMessage.Exception;
                return response;
            }
        }

        public async Task<IResponse<bool>> DeleteAttachment(int attachmentId)
        {
            var response = new Response<bool>();
            try
            {
                var attachment = await _appUow.RelativeAttachmentRepo.FindAsync(attachmentId);
                _appUow.RelativeAttachmentRepo.Delete(attachment);
                var deleteFileResult = await _appUow.ElkSaveChangesAsync();
                if (deleteFileResult.IsSuccessful)
                {
                    response.Result = true;
                    response.IsSuccessful = true;
                    response.Message = ServiceMessage.Success;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccessful = true;
                    response.Message = ServiceMessage.Error;
                }

                return response;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                response.Message = ServiceMessage.Exception;
                return response;
            }
        }

        public IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10)
        {
            var items = _RelativeRepo.Get(x => userId == null ? true : x.UserId == userId && (x.Name.Contains(searchParameter) || x.Family.Contains(searchParameter)), o => o.OrderByDescending(x => x.UserId));
            return items?.ToDictionary(k => (object)k.RelativeId, v => (object)$"{v.Name} {v.Family}({v.NationalCode})"); ;
        }

        public string Export(RelativeSearchFilter filter)
        {
            Expression<Func<Relative, bool>> conditions = x => true;
            if (filter != null)
            {
                if (filter.UserId != null)
                    conditions = conditions.And(x => x.UserId == filter.UserId);
                if (!string.IsNullOrWhiteSpace(filter.NationalCode))
                    conditions = conditions.And(x => x.NationalCode == filter.NationalCode);
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    conditions = conditions.And(x => (x.Name + " " + x.Family).Contains(filter.Name));
            }

            var items = _RelativeRepo.Get(conditions, x => x.OrderByDescending(u => u.RelativeId)); ;
            var sb = new StringBuilder(",Fullname,Relation,Gender,Takafol,National Code,Identity Number,FatherName,BirthDay,Register Date" + Environment.NewLine);
            int idx = 1;
            foreach (var item in items)
            {
                sb.Append($"{idx},{item.Fullname},{item.RelativeType.GetDescription()},{item.Gender.GetDescription()},{item.TakafolKind.GetDescription()},{item.NationalCode},{item.IdentityNumber},{item.FatherName},{item.BirthDay},{item.InsertDateSh}" + Environment.NewLine);
                idx++;
            }
            return sb.ToString();
        }
    }
}