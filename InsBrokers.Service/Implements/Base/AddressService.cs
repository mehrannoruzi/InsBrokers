using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using InsBrokers.DataAccess.Ef;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InsBrokers.Service.Resource;

namespace InsBrokers.Service
{
    public class AddressService : IAddressService
    {
        private readonly AppUnitOfWork _appUow;
        private readonly IGenericRepo<Address> _addressRepo;

        public AddressService(AppUnitOfWork appUOW)
        {
            _appUow = appUOW;
            _addressRepo = appUOW.AddressRepo;
        }

        //public IResponse<PagingListDetails<AddressSearchFilter>> Get(Guid userId)
        //{
        //    var currentDT = DateTime.Now;
        //    var addresses = _addressRepo.Get(selector: a => new AddressSearchFilter
        //    {
        //        UserId = userId,
        //        Details = a.AddressDetails
        //    },
        //    conditions: x => x.UserId == userId,
        //    pagingParameter: new PagingParameter
        //    {
        //        PageNumber = 1,
        //        PageSize = 3
        //    },
        //    orderBy: o => o.OrderByDescending(x => x.AddressId));
        //    return new Response<PagingListDetails<AddressSearchFilter>>
        //    {
        //        Result = addresses,
        //        IsSuccessful = true
        //    };
        //}

        public PagingListDetails<Address> Get(AddressSearchFilter filter)
        {
            Expression<Func<Address, bool>> conditions = x => true;
            if (filter != null)
            {
                if (filter.UserId != null)
                    conditions = conditions.And(x => x.UserId == filter.UserId);
                if (!string.IsNullOrWhiteSpace(filter.Details))
                    conditions = conditions.And(x => x.AddressDetails.Contains(filter.Details));
            }

            return _addressRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.AddressId), new System.Collections.Generic.List<Expression<Func<Address, object>>> {
                x=>x.User
            });
        }


        public async Task<IResponse<Address>> AddAsync(Address model)
        {
            await _appUow.AddressRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Address> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<Address>> UpdateAsync(Address model)
        {
            var address = await _addressRepo.FindAsync(model.AddressId);
            if (address == null) return new Response<Address> { Message = ServiceMessage.RecordNotExist };

            address.UserId = model.UserId;
            address.Province = model.Province;
            address.City = model.City;
            address.AddressDetails = model.AddressDetails;

            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<Address> { Result = address, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            _addressRepo.Delete(new Address { AddressId = id });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<Address>> FindAsync(int id)
        {
            var address = await _addressRepo.FirstOrDefaultAsync(x => x.AddressId == id, new System.Collections.Generic.List<Expression<Func<Address, object>>> {
                x=>x.User
            });
            if (address == null) return new Response<Address> { Message = ServiceMessage.RecordNotExist };
            return new Response<Address> { Result = address, IsSuccessful = true };
        }
    }
}