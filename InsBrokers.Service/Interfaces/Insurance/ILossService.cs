using Elk.Core;
using InsBrokers.Domain;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsBrokers.Service
{
    public interface ILossService
    {
        Task<IResponse<Loss>> AddAsync(Loss model, string root, IList<IFormFile> files);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<Loss>> FindAsync(int id);
        PagingListDetails<Loss> Get(LossSearchFilter filter);
        Task<IResponse<Loss>> UpdateAsync(Loss model, string root, IList<IFormFile> files);
    }
}