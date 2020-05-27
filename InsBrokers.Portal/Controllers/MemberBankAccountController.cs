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
    public partial class MemberBankAccountController : Controller
    {
        private readonly IBankAccountService _BankAccountSrv;

        public MemberBankAccountController(IBankAccountService BankAccountSrv)
        {
            _BankAccountSrv = BankAccountSrv;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.BankAccount}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new BankAccount()),
                AutoSubmitUrl = Url.Action("Add", "MemberBankAccount")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(BankAccount model)
        {
            model.UserId = User.GetUserId();
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _BankAccountSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _BankAccountSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.BankAccount) });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.BankAccount}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "MemberBankAccount"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(BankAccount model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _BankAccountSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _BankAccountSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(BankAccountSearchFilter filter)
        {
            filter.UserId = User.GetUserId();
            if (!Request.IsAjaxRequest()) return View(_BankAccountSrv.Get(filter));
            else return PartialView("Partials/_List", _BankAccountSrv.Get(filter));
        }

    }
}