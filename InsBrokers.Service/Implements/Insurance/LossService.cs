﻿using System;
using Elk.Core;
using Elk.Cache;
using System.Linq;
using InsBrokers.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InsBrokers.DataAccess.Ef;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using InsBrokers.Service.Resource;
using System.Text;

namespace InsBrokers.Service
{
    public class LossService : ILossService
    {
        private readonly ILossRepo _LossRepo;
        private readonly AppUnitOfWork _appUow;
        private readonly ILossAssetService _LossAssetSrv;
        private readonly IMemoryCacheProvider _cacheProvider;
        private string LossCountLastDaysCacheKey() => "LossCountLastDays";

        public LossService(AppUnitOfWork appUOW, ILossAssetService LossAssetSrv,
            IMemoryCacheProvider cacheProvider)
        {
            _appUow = appUOW;
            _LossRepo = appUOW.LossRepo;
            _LossAssetSrv = LossAssetSrv;
            _cacheProvider = cacheProvider;
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

        public async Task<IResponse<Loss>> AdminUpdateAsync(Loss model, string root, IList<IFormFile> files)
        {
            var Loss = await _LossRepo.FirstOrDefaultAsync(conditions: x => x.LossId == model.LossId, new List<Expression<Func<Loss, object>>> { i => i.User });
            if (Loss == null) return new Response<Loss> { Message = ServiceMessage.RecordNotExist };

            Loss.Status = model.Status;
            Loss.RelativeId = model.RelativeId;
            Loss.LossType = model.LossType;
            Loss.LossDateSh = model.LossDateSh;
            Loss.LossDateMi = PersianDateTime.Parse(model.LossDateSh).ToDateTime();
            //Loss.PatientName = model.PatientName;
            Loss.Cost = model.Cost;
            Loss.Description = model.Description;
            _LossRepo.Update(Loss);
            var getAssets = await _LossAssetSrv.SaveRange(root, model.UserId, files);
            if (!getAssets.IsSuccessful) return new Response<Loss> { Message = getAssets.Message };
            foreach (var item in getAssets.Result)
                item.LossId = Loss.LossId;
            await _appUow.LossAssetRepo.AddRangeAsync(getAssets.Result);
            var updateResult = await _appUow.ElkSaveChangesAsync();
            if (!updateResult.IsSuccessful) _LossAssetSrv.DeleteRange(getAssets.Result);
            return new Response<Loss> { Result = Loss, IsSuccessful = updateResult.IsSuccessful, Message = updateResult.Message };
        }


        public async Task<IResponse<Loss>> UpdateAsync(Loss model, string root, IList<IFormFile> files)
        {
            var Loss = await _LossRepo.FirstOrDefaultAsync(conditions: x => x.LossId == model.LossId, new List<Expression<Func<Loss, object>>> { i => i.User });
            if (Loss == null) return new Response<Loss> { Message = ServiceMessage.RecordNotExist };

            Loss.RelativeId = model.RelativeId;
            Loss.LossType = model.LossType;
            Loss.LossDateSh = model.LossDateSh;
            Loss.LossDateMi = PersianDateTime.Parse(model.LossDateSh).ToDateTime();
            Loss.Cost = model.Cost;
            Loss.Description = model.Description;
            var getAssets = await _LossAssetSrv.SaveRange(root, model.UserId, files);
            if (!getAssets.IsSuccessful) return new Response<Loss> { Message = getAssets.Message };
            foreach (var item in getAssets.Result)
                item.LossId = Loss.LossId;
            await _appUow.LossAssetRepo.AddRangeAsync(getAssets.Result);
            _LossRepo.Update(Loss);
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
            var Loss = await _LossRepo.FirstOrDefaultAsync(x => x.LossId == id, new List<Expression<Func<Loss, object>>> { x => x.LossAssets, x => x.User });
            if (Loss == null) return new Response<Loss> { Message = ServiceMessage.RecordNotExist };
            return new Response<Loss> { Result = Loss, IsSuccessful = true };
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
                if (filter.LossType.IsNotNull())
                    conditions = conditions.And(x => x.LossType == filter.LossType);
                if (!string.IsNullOrWhiteSpace(filter.NationalCode))
                    conditions = conditions.And(x => x.User.NationalCode == filter.NationalCode);

            }
            var result = _LossRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.LossId), new List<Expression<Func<Loss, object>>> { i => i.User, i => i.LossAssets });
            var ids = result.Items.Where(x => x.RelativeId != null).Select(x => x.RelativeId).ToList();
            var relatives = _appUow.RelativeRepo.Get(x => ids.Contains(x.RelativeId), o => o.OrderByDescending(x => x.RelativeId));
            foreach (var loss in result.Items.Where(x => x.RelativeId != null))
                loss.Relative = relatives.FirstOrDefault(x => x.RelativeId == loss.RelativeId);
            return result;
        }

        public async Task<IResponse<int>> GetLossCount()
        {
            var result = new Response<int>();
            try
            {
                result.Result = await _appUow.LossRepo.GetLossCount();

                result.IsSuccessful = true;
                result.Message = ServiceMessage.Success;
                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return result;
            }
        }

        public async Task<IResponse<Dictionary<string, int>>> GetLossCountLastDaysAsync(int dayCount = 10)
        {
            var result = new Response<Dictionary<string, int>>();
            try
            {
                var cache = (Response<Dictionary<string, int>>)_cacheProvider.Get(LossCountLastDaysCacheKey());
                if (cache != null) return cache;

                result.Result = await _appUow.LossRepo.GetLossCountLastDaysAsync(dayCount);
                if (result.Result.Count() == 0) return new Response<Dictionary<string, int>> { Message = ServiceMessage.RecordNotExist };

                result.IsSuccessful = true;
                result.Message = ServiceMessage.Success;
                _cacheProvider.Add(LossCountLastDaysCacheKey(), result, DateTimeOffset.Now.AddMinutes(30));

                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return result;
            }
        }

        public string Export(LossSearchFilter filter)
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
                if (filter.LossType.IsNotNull())
                    conditions = conditions.And(x => x.LossType == filter.LossType);
                if (!string.IsNullOrWhiteSpace(filter.NationalCode))
                    conditions = conditions.And(x => x.User.NationalCode == filter.NationalCode);

            }
            var result = _LossRepo.Get(conditions, x => x.OrderByDescending(u => u.LossId), new List<Expression<Func<Loss, object>>> { i => i.User, i => i.LossAssets });
            var ids = result.Where(x => x.RelativeId != null).Select(x => x.RelativeId).ToList();
            var relatives = _appUow.RelativeRepo.Get(x => ids.Contains(x.RelativeId), o => o.OrderByDescending(x => x.RelativeId));
            var sb = new StringBuilder(",User,Mobile Number,National Code,Patient Name,Cost Type,Cost,Date,Status,Description" + Environment.NewLine);
            int idx = 1;
            Relative relative = null;
            string relativeName = string.Empty;
            foreach (var loss in result)
            {
                if (loss.RelativeId != null)
                {
                    relative = relatives.FirstOrDefault(x => x.RelativeId == loss.RelativeId);
                    relativeName = relative == null ? ServiceMessage.OriginalInsured : relative.Fullname;
                }
                else relativeName = ServiceMessage.OriginalInsured;
                sb.Append($"{idx},{loss.User.Fullname},{loss.User.MobileNumber},{loss.User.NationalCode},{relativeName},{loss.LossType.GetDescription()},{loss.Cost.ToString("N0")},{loss.LossDateSh},{loss.Status.GetDescription()},{loss.Description}" + Environment.NewLine);
                idx++;
            }
            return sb.ToString();
        }
    }
}