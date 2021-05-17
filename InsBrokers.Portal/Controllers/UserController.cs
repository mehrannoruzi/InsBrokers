using System;
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
    public partial class UserController : Controller
    {
        private readonly IUserService _userSrv;

        public UserController(IUserService userBiz)
        {
            _userSrv = userBiz;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.User}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new User()),
                AutoSubmitUrl = Url.Action("Add", "User")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(User model)
        {
            ModelState.Remove(nameof(model.LastLoginDateSh));
            ModelState.Remove(nameof(model.InsertDateSh));
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _userSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(Guid id)
        {
            var findUser = await _userSrv.FindAsync(id);
            if (!findUser.IsSuccessful) return Json(new Response<string> { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.User) });
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.User}",
                AutoSubmitBtnText = Strings.Edit,
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", findUser.Result),
                AutoSubmitUrl = Url.Action("Update", "User"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(User model)
        {
            ModelState.Remove(nameof(model.LastLoginDateSh));
            ModelState.Remove(nameof(model.InsertDateSh));
            ModelState.Remove(nameof(model.Password));
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _userSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(Guid id) => Json(await _userSrv.DeleteAsync(id));

        [HttpGet]
        public virtual async Task<ActionResult> ProfileInfo()
                => View((await _userSrv.FindAsync(User.GetUserId())).Result);

        [HttpPost]
        public virtual async Task<JsonResult> ProfileInfo(User model)
        {
            if (User.GetUserId() != model.UserId) return Json(new Response<string> { Message = Strings.Error });
            if (!string.IsNullOrWhiteSpace(model.NewPassword) && model.NewPassword.Length < 5) return Json(new Response<string> { Message = Strings.PasswordValidationFailed });
            return Json(await _userSrv.UpdateProfile(model));
        }

        [HttpGet]
        public virtual ActionResult Manage(UserSearchFilter filter)
        {
            ViewBag.ExcelExport = true;
            if (!Request.IsAjaxRequest()) return View(_userSrv.Get(filter));
            else return PartialView("Partials/_List", _userSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("UserInRole", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_userSrv.Search(q).ToSelectListItems());

        [HttpGet]
        public virtual async Task<IActionResult> Details(Guid id)
        {
            var foundUser = await _userSrv.FindAsync(id);
            if (!foundUser.IsSuccessful)
                return Json(new Modal { IsSuccessful = false, Message = Strings.NotFound });
            return Json(new Modal
            {
                IsSuccessful = true,
                Title = $"{Strings.Details} {DomainString.User}",
                AutoSubmitBtnText = Strings.Edit,
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Details", foundUser.Result),
                AutoSubmit = false
            });
        }

        //[HttpGet, AuthEqualTo("User", "Manage")]
        //public virtual JsonResult Excel(UserSearchFilter filter)
        //        => Json(_userSrv.Export(filter));

        [HttpGet, AuthEqualTo("User", "Manage")]
        public virtual IActionResult Excel(UserSearchFilter filter)
        {
            try
            {
                string fileName = $"Customers_{PersianDateTime.Now.ToString(PersianDateTimeFormat.Date).Replace("/", "-")}.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileContent = _userSrv.ExportExcel(filter);
                return File(fileContent, contentType, fileName);
            }
            catch
            {
                return null;
            }
        }

    }
}