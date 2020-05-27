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
    //[AuthorizationFilter]
    public partial class MemberAddressController : Controller
    {
        private readonly IAddressService _MemberAddressSrv;

        public MemberAddressController(IAddressService MemberAddressSrv)
        {
            _MemberAddressSrv = MemberAddressSrv;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Address}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Address()),
                AutoSubmitUrl = Url.Action("Add", "MemberAddress")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Address model)
        {
            model.UserId = User.GetUserId();
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _MemberAddressSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _MemberAddressSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Address) });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Address}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "MemberAddress"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Address model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _MemberAddressSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _MemberAddressSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(AddressSearchFilter filter)
        {
            filter.UserId = User.GetUserId();
            if (!Request.IsAjaxRequest()) return View(_MemberAddressSrv.Get(filter));
            else return PartialView("Partials/_List", _MemberAddressSrv.Get(filter));
        }

    }
}