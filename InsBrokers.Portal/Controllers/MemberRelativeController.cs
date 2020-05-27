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
    public partial class MemberRelativeController : Controller
    {
        private readonly IRelativeService _MemberRelativeSrv;

        public MemberRelativeController(IRelativeService MemberRelativeSrv)
        {
            _MemberRelativeSrv = MemberRelativeSrv;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Relatives}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Relative()),
                AutoSubmitUrl = Url.Action("Add", "MemberRelative")
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

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Relatives}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "MemberRelative"),
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
            if (!Request.IsAjaxRequest()) return View(_MemberRelativeSrv.Get(filter));
            else return PartialView("Partials/_List", _MemberRelativeSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("MemberRelative", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_MemberRelativeSrv.Search(q,User.GetUserId()).ToSelectListItems());
    }
}