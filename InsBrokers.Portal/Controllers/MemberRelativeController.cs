using Elk.Core;
using Elk.Http;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using Microsoft.Extensions.Configuration;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{
    [AuthorizationFilter]
    public partial class MemberRelativeController : Controller
    {
        private readonly IUserService _userSrv;
        private readonly IConfiguration _configuration;
        private readonly IRelativeService _MemberRelativeSrv;

        public MemberRelativeController(IUserService userSrv, IRelativeService MemberRelativeSrv,
            IConfiguration configuration)
        {
            _userSrv = userSrv;
            _configuration = configuration;
            _MemberRelativeSrv = MemberRelativeSrv;
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
        public virtual async Task<ActionResult> Manage(RelativeSearchFilter filter)
        {
            filter.UserId = User.GetUserId();
            //ViewBag.HasExtraButton = true;
            //ViewBag.ExtraButtonUrl = Url.Action("InsuranceInfo", "User");
            //ViewBag.ExtraButtonText = Strings.PreFactor;
            //ViewBag.ExtraButtonIcon = "zmdi-eye";

            var getUser = await _userSrv.FindAsync(User.GetUserId());
            var startDate = PersianDateTime.Parse(_configuration["CustomSettings:StartLockDate"]).ToDateTime();
            var endDate = PersianDateTime.Parse(_configuration["CustomSettings:EndLockDate"]).ToDateTime();
            ViewBag.CanEdit = !(getUser.Result.InsertDateMi >= startDate && getUser.Result.InsertDateMi <= endDate);
            ViewBag.WithoutAddButton = !ViewBag.CanEdit;

            if (!Request.IsAjaxRequest()) return View(_MemberRelativeSrv.Get(filter));
            else return PartialView("Partials/_List", _MemberRelativeSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("MemberRelative", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_MemberRelativeSrv.Search(q, User.GetUserId()).ToSelectListItems());
    }
}