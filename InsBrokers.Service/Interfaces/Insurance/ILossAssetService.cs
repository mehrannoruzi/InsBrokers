using Elk.Core;
using InsBrokers.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsBrokers.Service
{
    public interface ILossAssetService
    {
        IResponse<string> DeleteRange(IList<LossAsset> assets);
        Task<IResponse<IList<LossAsset>>> SaveRange(string root, Guid userId, IList<IFormFile> files);
        IResponse<string> DeleteRange(int LossId);
        Task<IResponse<string>> DeleteAsync(int id);
    }
}