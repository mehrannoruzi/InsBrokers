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
    public partial class AddressController : Controller
    {
        private readonly IAddressService _addressSrv;

        public AddressController(IAddressService addressSrv)
        {
            _addressSrv = addressSrv;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Address}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Address()),
                AutoSubmitUrl = Url.Action("Add", "Address")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Address model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _addressSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _addressSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Address) });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Address}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "Address"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Address model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _addressSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _addressSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(AddressSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_addressSrv.Get(filter));
            else return PartialView("Partials/_List", _addressSrv.Get(filter));
        }

    }
}