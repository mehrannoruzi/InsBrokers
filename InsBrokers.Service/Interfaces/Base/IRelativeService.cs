using Elk.Core;
using InsBrokers.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsBrokers.Service
{
    public interface IRelativeService
    {
        Task<IResponse<Relative>> AddAsync(Relative model);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<Relative>> FindAsync(int roleId);
        PagingListDetails<Relative> Get(RelativeSearchFilter filter);
        Task<IResponse<Relative>> UpdateAsync(Relative model);
        IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10);
        string Export(RelativeSearchFilter filter);
    }
}