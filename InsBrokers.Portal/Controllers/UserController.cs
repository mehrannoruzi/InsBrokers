using System;
using Elk.Core;
using Elk.Http;
using System.IO;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using Microsoft.Extensions.Configuration;
using DomainString = InsBrokers.Domain.Resource.Strings;
using Microsoft.AspNetCore.Authorization;

namespace InsBrokers.Portal.Controllers
{
    //[AuthorizationFilter]
    public partial class UserController : Controller
    {
        private readonly IUserService _userSrv;
        private readonly IRelativeService _relativeSrv;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userSrv, IRelativeService relativeSrv, IConfiguration configuration)
        {
            _userSrv = userSrv;
            _relativeSrv = relativeSrv;
            _configuration = configuration;
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

        [HttpPost, AllowAnonymous]
        public virtual async Task<JsonResult> DeleteAttachment(int attachmentId) => Json(await _userSrv.DeleteAttachment(attachmentId));

        [HttpGet]
        public virtual async Task<ActionResult> ProfileInfo()
        {
            var user = await _userSrv.FindWithAttachmentsAsync(User.GetUserId());
            foreach (var item in user.Result.UserAttachments)
                item.Url = $"{_configuration["CustomSettings:MainAddress"]}{item.Url}";

            return View(user.Result);
        }

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

        [HttpGet]
        public virtual async Task<IActionResult> InsuranceInfo()
            => View(await _userSrv.GetInsuranceInfo(User.GetUserId()));

        [HttpGet]
        public async Task<ActionResult> DownloadAllAttachments(Guid id)
        {
            var user = await _userSrv.FindWithAttachmentsAsync(id);
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var attchment in user.Result.UserAttachments)
                    {
                        try
                        {
                            var filePath = $"wwwroot/{attchment.Url}";
                            var fileContent = await System.IO.File.ReadAllBytesAsync(filePath);


                            var zipArchiveEntry = zipArchive.CreateEntry($"CustomerAttachments/{Path.GetFileName(filePath)}", CompressionLevel.Optimal);
                            using (var zipStream = zipArchiveEntry.Open())
                                zipStream.Write(fileContent, 0, fileContent.Length);
                        }
                        catch (Exception e) { }
                    }

                    foreach (var relative in user.Result.Relatives)
                    {
                        var userRelative = await _relativeSrv.FindWithAttachmentsAsync(relative.RelativeId);
                        foreach (var attchment in userRelative.Result.RelativeAttachments)
                        {
                            try
                            {
                                var filePath = $"wwwroot/{attchment.Url}";
                                var fileContent = await System.IO.File.ReadAllBytesAsync(filePath);


                                var zipArchiveEntry = zipArchive.CreateEntry($"CustomerRelativesAttachments/{relative.NationalCode}/{Path.GetFileName(filePath)}", CompressionLevel.Optimal);
                                using (var zipStream = zipArchiveEntry.Open())
                                    zipStream.Write(fileContent, 0, fileContent.Length);
                            }
                            catch (Exception e) { }
                        }
                    }
                }

                return File(memoryStream.ToArray(), "application/zip", $"{user.Result.NationalCode}-{user.Result.MobileNumber}.zip");
            }
        }
    }
}