using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InsBrokers.DataAccess.Ef;
using System.Collections.Generic;
using InsBrokers.Service.Resource;

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
            await _appUow.RelativeRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Relative> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<Relative>> UpdateAsync(Relative model)
        {
            var Relative = await _RelativeRepo.FindAsync(model.RelativeId);
            if (Relative == null) return new Response<Relative> { Message = ServiceMessage.RecordNotExist };

            Relative.Name = model.Name;
            Relative.Family = model.Family;
            Relative.FatherName = model.FatherName;
            Relative.BirthDay = model.BirthDay;
            Relative.BirthDayMi = PersianDateTime.Parse(model.BirthDay).ToDateTime();
            Relative.IdentityNumber = model.IdentityNumber;
            Relative.NationalCode = model.NationalCode;
            Relative.RelativeType = model.RelativeType;
            Relative.TakafolKind = model.TakafolKind;
            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<Relative> { Result = Relative, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
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
            var Relative = await _RelativeRepo.FindAsync(id);
            if (Relative == null) return new Response<Relative> { Message = ServiceMessage.RecordNotExist };
            return new Response<Relative> { Result = Relative, IsSuccessful = true };
        }

        public IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10)
        {
            var items = _RelativeRepo.Get(x => userId == null ? true : x.UserId == userId && x.Name.Contains(searchParameter) || x.Family.Contains(searchParameter), o => o.OrderByDescending(x => x.UserId));
            return items?.ToDictionary(k => (object)k.RelativeId, v => (object)$"{v.Name} {v.Family}({v.NationalCode})"); ;
        }
    }
}