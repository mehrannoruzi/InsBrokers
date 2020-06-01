using System;
using Elk.Core;
using Elk.Http;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using InsBrokers.Portal.Resource;
using Microsoft.AspNetCore.Hosting;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{
    [AuthorizationFilter]
    public partial class MemberLossController : Controller
    {
        private readonly ILossService _LossSrv;
        private readonly IWebHostEnvironment _env;

        public MemberLossController(ILossService LossSrv, IWebHostEnvironment env)
        {
            _LossSrv = LossSrv;
            _env = env;
        }



        private IEnumerable<LossDescriptionDTO> GetTypes()
        {
            try
            {
                return System.IO.File.ReadAllText(System.IO.Path.Combine(_env.ContentRootPath + "/LossDescription.json")).DeSerializeJson<List<LossDescriptionDTO>>();
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new List<LossDescriptionDTO>();
            }
        }

        [HttpGet]
        public virtual JsonResult Add()
        {
            ViewBag.Types = GetTypes();
            return Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Loss}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Loss()),
                AutoSubmit = false
            });
        }


        [HttpPost]
        public virtual async Task<JsonResult> Add(LossAddModel model)
        {
            model.UserId = User.GetUserId();
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            var save = await _LossSrv.AddAsync(model, _env.ContentRootPath, model.Files);
            return Json(new { save.IsSuccessful, save.Message });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update([FromServices]IRelativeService relativeSrv,int id)
        {
            var find = await _LossSrv.FindAsync(id);
            if (!find.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Loss) });
            ViewBag.Types = GetTypes();
            if(find.Result.RelativeId!=null)
            {
                var rel = await relativeSrv.FindAsync(find.Result.RelativeId ?? 0);
                find.Result.Relative = rel.Result;
            }

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Loss}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", find.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Loss model, IList<IFormFile> files)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            var update = await _LossSrv.UpdateAsync(model, _env.ContentRootPath, files);
            return Json(new { update.IsSuccessful,update.Message });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _LossSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(LossSearchFilter filter)
        {
            ViewBag.Types = GetTypes();
            filter.UserId = User.GetUserId();
            if (!Request.IsAjaxRequest()) return View(_LossSrv.Get(filter));
            else return PartialView("Partials/_List", _LossSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("MemberLoss", "Manage")]
        public virtual async Task<JsonResult> DeleteAsset([FromServices] ILossAssetService assetSrv, int assetId)
            => Json(await assetSrv.DeleteAsync(assetId));

    }
}