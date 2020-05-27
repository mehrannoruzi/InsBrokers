using System;
using Elk.Http;
using Elk.Core;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InsBrokers.Portal.Resource;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{
    [AuthorizationFilter]
    public partial class LossController : Controller
    {
        private readonly ILossService _LossSrv;
        private readonly IWebHostEnvironment _env;
        public LossController(ILossService LossSrv, IWebHostEnvironment env)
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
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            var save = await _LossSrv.AddAsync(model, _env.ContentRootPath, model.Files);
            return Json(new { save.IsSuccessful, save.Message });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _LossSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Loss) });
            ViewBag.Types = GetTypes();
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Loss}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Loss model, IList<IFormFile> files)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            var update = await _LossSrv.AdminUpdateAsync(model, _env.ContentRootPath, files);
            return Json(new { update.IsSuccessful, update.Message });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _LossSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(LossSearchFilter filter)
        {
            ViewBag.WithoutAddButton = true;
            ViewBag.Types = GetTypes();
            if (!Request.IsAjaxRequest()) return View(_LossSrv.Get(filter));
            else return PartialView("Partials/_List", _LossSrv.Get(filter));
        }

        [HttpPost]
        public virtual async Task<JsonResult> DeleteAsset([FromServices] ILossAssetService assetSrv, int assetId)
            => Json(await assetSrv.DeleteAsync(assetId));

        [HttpGet, AuthEqualTo("Loss", "Update")]
        public virtual async Task<JsonResult> Details(int id)
        {
            var findRep = await _LossSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Loss) });
            ViewBag.Types = GetTypes();
            return Json(new Modal
            {
                Title = $"{Strings.Details} {DomainString.Loss}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Details", findRep.Result),
                AutoSubmit = false
            });
        }

        [HttpGet, AuthEqualTo("Loss", "Add")]
        public virtual JsonResult Search([FromServices]IRelativeService relativeSrv, string q)
        => Json(relativeSrv.Search(q, User.GetUserId()).ToSelectListItems());
    }
}