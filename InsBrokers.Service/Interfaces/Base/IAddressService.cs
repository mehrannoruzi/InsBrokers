using System;
using System.Threading.Tasks;
using Elk.Core;
using InsBrokers.Domain;

namespace InsBrokers.Service
{
    public interface IAddressService
    {
        IResponse<PagingListDetails<AddressDTO>> Get(Guid userId);
        PagingListDetails<Address> Get(AddressSearchFilter filter);
        Task<IResponse<Address>> AddAsync(Address model);
        Task<IResponse<Address>> UpdateAsync(Address model);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<Address>> FindAsync(int roleId);
    }
}