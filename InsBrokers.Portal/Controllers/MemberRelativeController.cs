using Elk.Core;
using Elk.Http;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using DomainString = InsBrokers.Domain.Resource.Strings;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace InsBrokers.Portal.Controllers
{
    [AuthorizationFilter]
    public partial class MemberRelativeController : Controller
    {
        private readonly IRelativeService _MemberRelativeSrv;
        private readonly IConfiguration _configuration;
        public MemberRelativeController(IRelativeService MemberRelativeSrv, IConfiguration configuration)
        {
            _MemberRelativeSrv = MemberRelativeSrv;
            _configuration = configuration;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Relatives}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Relative()),
                AutoSubmit = false,
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Relative model)
        {
            model.UserId = User.GetUserId();
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _MemberRelativeSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _MemberRelativeSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Relatives) });
            if (findRep.Result.RelativeAttachments != null)
                foreach (var item in findRep.Result.RelativeAttachments)
                    item.Url = $"{_configuration["CustomSettings:MainAddress"]}{item.Url}";
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Relatives}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false,
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Relative model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _MemberRelativeSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _MemberRelativeSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(RelativeSearchFilter filter)
        {
            filter.UserId = User.GetUserId();
            //ViewBag.HasExtraButton = true;
            //ViewBag.ExtraButtonUrl = Url.Action("InsuranceInfo", "User");
            //ViewBag.ExtraButtonText = Strings.PreFactor;
            //ViewBag.ExtraButtonIcon = "zmdi-eye";
            if (!Request.IsAjaxRequest()) return View(_MemberRelativeSrv.Get(filter));
            else return PartialView("Partials/_List", _MemberRelativeSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("MemberRelative", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_MemberRelativeSrv.Search(q, User.GetUserId()).ToSelectListItems());
    }
}