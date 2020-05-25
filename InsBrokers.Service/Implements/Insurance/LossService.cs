using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using InsBrokers.DataAccess.Ef;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InsBrokers.Service.Resource;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace InsBrokers.Service
{
    public class LossService : ILossService
    {
        private readonly AppUnitOfWork _appUow;
        private readonly IGenericRepo<Loss> _LossRepo;
        private readonly ILossAssetService _LossAssetSrv;

        public LossService(AppUnitOfWork appUOW, ILossAssetService LossAssetSrv)
        {
            _appUow = appUOW;
            _LossRepo = appUOW.LossRepo;
            _LossAssetSrv = LossAssetSrv;
        }

        public PagingListDetails<Loss> Get(LossSearchFilter filter)
        {
            Expression<Func<Loss, bool>> conditions = x => true;
            if (filter != null)
            {
                if (filter.UserId != null)
                    conditions = conditions.And(x => x.UserId == filter.UserId);
                if (!string.IsNullOrWhiteSpace(filter.LossDateShFrom))
                {
                    var from = PersianDateTime.Parse(filter.LossDateShFrom).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi >= from);
                }
                if (!string.IsNullOrWhiteSpace(filter.LossDateShFrom))
                {
                    var to = PersianDateTime.Parse(filter.LossDateShFrom).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi <= to);
                }
                if (!string.IsNullOrWhiteSpace(filter.LossType))
                    conditions = conditions.And(x => x.LossType == filter.LossType);
                if (!string.IsNullOrWhiteSpace(filter.PatientName))
                    conditions = conditions.And(x => x.PatientName.Contains(filter.PatientName));

            }

            return _LossRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.LossId), new List<Expression<Func<Loss, object>>> { i => i.User, i => i.LossAssets }); ;
        }


        public async Task<IResponse<Loss>> AddAsync(Loss model, string root, IList<IFormFile> files)
        {
            var getAssets = await _LossAssetSrv.SaveRange(root, model.UserId, files);
            if (!getAssets.IsSuccessful) return new Response<Loss> { Message = getAssets.Message };
            model.LossAssets = getAssets.Result;
            await _appUow.LossRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            if (!saveResult.IsSuccessful) _LossAssetSrv.DeleteRange(getAssets.Result);
            return new Response<Loss> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<Loss>> UpdateAsync(Loss model, string root, IList<IFormFile> files)
        {
            var Loss = await _LossRepo.FindAsync(model.LossId);
            if (Loss == null) return new Response<Loss> { Message = ServiceMessage.RecordNotExist };

            Loss.RelationType = model.RelationType;
            Loss.LossType = model.LossType;
            Loss.LossDateSh = model.LossDateSh;
            Loss.LossDateMi = PersianDateTime.Parse(model.LossDateSh).ToDateTime();
            Loss.PatientName = model.PatientName;
            Loss.Cost = model.Cost;
            Loss.Description = model.Description;
            var getAssets = await _LossAssetSrv.SaveRange(root, model.UserId, files);
            if (!getAssets.IsSuccessful) return new Response<Loss> { Message = getAssets.Message };
            foreach (var item in getAssets.Result)
                item.LossId = Loss.LossId;
            await _appUow.LossAssetRepo.AddRangeAsync(getAssets.Result);
            var updateResult = await _appUow.ElkSaveChangesAsync();
            if (!updateResult.IsSuccessful) _LossAssetSrv.DeleteRange(getAssets.Result);
            return new Response<Loss> { Result = Loss, IsSuccessful = updateResult.IsSuccessful, Message = updateResult.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            _LossRepo.Delete(new Loss { LossId = id });
            var delete = await _appUow.ElkSaveChangesAsync();
            _LossAssetSrv.DeleteRange(id);
            return new Response<bool>
            {
                Message = delete.Message,
                Result = delete.IsSuccessful,
                IsSuccessful = delete.IsSuccessful,
            };
        }

        public async Task<IResponse<Loss>> FindAsync(int id)
        {
            var Loss = await _LossRepo.FirstOrDefaultAsync(x => x.LossId == id, new List<Expression<Func<Loss, object>>> { x => x.LossAssets });
            if (Loss == null) return new Response<Loss> { Message = ServiceMessage.RecordNotExist };
            return new Response<Loss> { Result = Loss, IsSuccessful = true };
        }
    }
}