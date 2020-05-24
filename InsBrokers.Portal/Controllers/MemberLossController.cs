using Elk.Core;
using InsBrokers.Domain;
using InsBrokers.Service;
using System;
using Elk.Http;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{
    // [AuthorizationFilter]
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
        public virtual async Task<JsonResult> Add(Loss model, IList<IFormFile> files)
        {
            model.UserId = User.GetUserId();
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _LossSrv.AddAsync(model, _env.ContentRootPath, files));
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
            return Json(await _LossSrv.UpdateAsync(model, _env.ContentRootPath, files));
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

        [HttpPost]
        public virtual async Task<JsonResult> DeleteAssetAsync([FromServices] ILossAssetService assetSrv, int assetId)
            => Json(await assetSrv.DeleteAsync(assetId));

    }
}