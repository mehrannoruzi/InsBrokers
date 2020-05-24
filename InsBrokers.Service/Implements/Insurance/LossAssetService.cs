using System;
using Elk.Core;
using InsBrokers.Domain;
using InsBrokers.DataAccess.Ef;
using System.Threading.Tasks;
using InsBrokers.Service.Resource;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace InsBrokers.Service
{
    public class LossAssetService : ILossAssetService
    {
        private readonly AppUnitOfWork _appUow;
        private readonly IGenericRepo<LossAsset> _lossAssetRepo;
        public LossAssetService(AppUnitOfWork appUOW)
        {
            _appUow = appUOW;
            _lossAssetRepo = appUOW.LossAssetRepo;
        }

        public async Task<IResponse<IList<LossAsset>>> SaveRange(string root, Guid userId, IList<IFormFile> files)
        {
            try
            {
                var items = new List<LossAsset>();
                var id = userId.ToString().Replace("-", "_");
                var pdt = PersianDateTime.Now;
                var dir = $"/Files/{id}/{pdt.Year}/{pdt.Month}";
                if (FileOperation.CreateDirectory(Path.Combine(root, dir)))
                    return new Response<IList<LossAsset>> { Message = ServiceMessage.SaveFileFailed };
                foreach (var file in files)
                {
                    var relativePath = $"{dir}/{Guid.NewGuid().ToString().Replace("-", "_")}{Path.GetExtension(file.FileName)}";
                    var phisicalPath = Path.Combine(root, relativePath);
                    items.Add(new LossAsset
                    {
                        Name = file.Name,
                        Extention = Path.GetExtension(file.FileName),
                        FileType = FileOperation.GetFileType(file.FileName),
                        FileUrl = "~" + relativePath,
                        PhysicalPath = phisicalPath
                    });
                    using (var stream = File.Create(phisicalPath))
                        await file.CopyToAsync(stream);
                }

                return new Response<IList<LossAsset>> { IsSuccessful = true, Result = items };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<IList<LossAsset>> { Message = ServiceMessage.SaveFileFailed };
            }

        }

        public IResponse<string> DeleteRange(IList<LossAsset> assets)
        {
            try
            {
                foreach (var asset in assets)
                {
                    if (File.Exists(asset.PhysicalPath))
                        File.Delete(asset.PhysicalPath);
                }
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }

        public IResponse<string> DeleteRange(int LossId)
        {
            try
            {
                foreach (var asset in _lossAssetRepo.Get(conditions: x => x.LossId == LossId, null))
                {
                    if (File.Exists(asset.PhysicalPath))
                        File.Delete(asset.PhysicalPath);
                }
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }

        public async Task<IResponse<string>> DeleteAsync(int id)
        {
            try
            {
                var asset = await _lossAssetRepo.FindAsync(id);
                if(asset == null)
                    return new Response<string> { Message = ServiceMessage.RecordNotExist };
                if (File.Exists(asset.PhysicalPath))
                    File.Delete(asset.PhysicalPath);
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }
    }
}