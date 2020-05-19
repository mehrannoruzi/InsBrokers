using System;
using Elk.Core;
using InsBrokers.Domain;

namespace InsBrokers.Service
{
    public interface IAddressService
    {
        IResponse<PagingListDetails<AddressDTO>> Get(Guid userId);
    }
}