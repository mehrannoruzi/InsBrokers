using Elk.Core;
using Elk.Http;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{
    [AuthorizationFilter]
    public partial class RelativeController : Controller
    {
        private readonly IRelativeService _relativeSrv;

        public RelativeController(IRelativeService MemberRelativeSrv)
        {
            _relativeSrv = MemberRelativeSrv;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Relatives}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Relative()),
                AutoSubmitUrl = Url.Action("Add", "Relative")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Relative model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _relativeSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _relativeSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Relatives) });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Relatives}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "Relative"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Relative model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _relativeSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _relativeSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(RelativeSearchFilter filter)
        {
            ViewBag.ExcelExport = true;
            if (!Request.IsAjaxRequest()) return View(_relativeSrv.Get(filter));
            else return PartialView("Partials/_List", _relativeSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("Relative", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_relativeSrv.Search(q,User.GetUserId()).ToSelectListItems());

        [HttpGet, AuthEqualTo("Relative", "Manage")]
        public virtual JsonResult Excel(RelativeSearchFilter filter)
            => Json(_relativeSrv.Export(filter));
    }
}