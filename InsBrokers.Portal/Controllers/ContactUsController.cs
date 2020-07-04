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
   // [AuthorizationFilter]
    public partial class ContactUsController : Controller
    {
        private readonly IContactUsService _ContactUsSrv;

        public ContactUsController(IContactUsService ContactUsSrv)
        {
            _ContactUsSrv = ContactUsSrv;
        }


        //[HttpGet]
        //public virtual JsonResult Add()
        //    => Json(new Modal
        //    {
        //        Title = $"{Strings.Add} {DomainString.ContactUs}",
        //        Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new ContactUs { Enabled = true }),
        //        AutoSubmitUrl = Url.Action("Add", "ContactUs")
        //    });

        //[HttpPost]
        //public virtual async Task<JsonResult> Add(ContactUs model)
        //{
        //    if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
        //    return Json(await _ContactUsSrv.AddAsync(model));
        //}

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _ContactUsSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.ContactUs) });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.ContactUs}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "ContactUs"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(ContactUs model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _ContactUsSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _ContactUsSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(ContactUsSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_ContactUsSrv.Get(filter));
            else return PartialView("Partials/_List", _ContactUsSrv.Get(filter));
        }

    }
}