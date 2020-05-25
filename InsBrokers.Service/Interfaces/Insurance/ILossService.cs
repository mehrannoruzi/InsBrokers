using Elk.Core;
using InsBrokers.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InsBrokers.Service
{
    public interface ILossService
    {
        Task<IResponse<Loss>> AddAsync(Loss model, string root, IList<IFormFile> files);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<Loss>> FindAsync(int id);
        PagingListDetails<Loss> Get(LossSearchFilter filter);
        Task<IResponse<Loss>> AdminUpdateAsync(Loss model, string root, IList<IFormFile> files);
        Task<IResponse<Loss>> UpdateAsync(Loss model, string root, IList<IFormFile> files);
        Task<IResponse<int>> GetLossCount();
        Task<IResponse<Dictionary<string, int>>> GetLossCountLastDaysAsync(int dayCount = 10);
    }
}