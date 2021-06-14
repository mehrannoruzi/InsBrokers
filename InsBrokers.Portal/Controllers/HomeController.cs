using Elk.Core;
using System.IO;
using System.Linq;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using InsBrokers.Portal.Resource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace InsBrokers.Portal.Controllers
{
    public class HomeController : BaseAuthController
    {
        private IConfiguration _configuration;
        private readonly IUserService _userSrv;
        private readonly IRelativeService _relativeSrv;

        public HomeController(IUserService userSrv, IRelativeService relativeSrv,
            IConfiguration configuration, IHttpContextAccessor httpAccessor) : base(httpAccessor)
        {
            _userSrv = userSrv;
            _relativeSrv = relativeSrv;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> AddUserAttachments(AttachmentModel model)
            => Json(await _userSrv.AddAttachments(model.File, model.Type));

        [HttpPost]
        public virtual async Task<JsonResult> AddRelativeAttachments(AttachmentModel model)
            => Json(await _relativeSrv.AddAttachments(User.GetUserId(), model.File, model.Type));

        [HttpGet]
        public IActionResult Register()
        {
            var organization = _configuration["CustomSettings:Organizations"].Split(";");
            var organizationList = new List<SelectListItem> { new SelectListItem { Text = "", Value = "", Selected = true } };
            foreach (var item in organization)
                organizationList.Add(new SelectListItem { Text = item, Value = item, Selected = false });

            var plans = _configuration["InsurancePlanSettings:Plans"].Split(";");
            var insurancePlanList = new List<SelectListItem> { new SelectListItem { Text = "", Value = "", Selected = true } };
            foreach (var item in plans)
                insurancePlanList.Add(new SelectListItem { Text = item, Value = item, Selected = false });

            ViewBag.OrganizationList = organizationList;
            ViewBag.InsurancePlan = insurancePlanList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] PortalSignUpModel model)
        {
            try
            {
                FileLoger.CriticalInfo($"Register Clicked");
                FileLoger.CriticalInfo(model.SerializeToJson());

                if (!ModelState.IsValid) return Json(new Response<string> { Message = ModelState.GetModelError() });

                FileLoger.CriticalInfo($"ModelState IsValid");

                if (model.UserAttachmentIds.Count() < 3) return Json(new Response<string> { Message = Strings.MustUploadAttachments });
                
                FileLoger.CriticalInfo($"Attachment IsValid");

                model.MemberRoleId = int.Parse(_configuration["CustomSettings:MemberRoleId"]);
                var save = await _userSrv.SignUp(model);
                if (!save.IsSuccessful) return Json(save);

                FileLoger.CriticalInfo($"SignUp Success");

                var menuRep = _userSrv.GetAvailableActions(save.Result.UserId, null, _configuration["CustomSettings:UrlPrefix"]);
                if (menuRep == null) return Json(new Response<string> { IsSuccessful = false, Message = Strings.ThereIsNoViewForUser });

                await CreateCookie(save.Result, true);

                FileLoger.CriticalInfo($"Register Success");

                return Json(new { IsSuccessful = true, Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { }) });
            }
            catch (Exception e)
            {
                FileLoger.CriticalInfo($"Message: {e.Message} {Environment.NewLine} InnerMessage: {e.InnerException?.Message} ");

                FileLoger.CriticalError(e);
                return Json(new Response<string> { Message = Strings.Error });
            }
        }

        [HttpGet]
        public async Task<FileResult> Android([FromServices] IWebHostEnvironment env)
        {
            try
            {
                var path = Path.Combine(env.WebRootPath, "Files", "DarmanNaft.apk");

                FileLoger.Info($"Download Android Clicked: {path}");

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.android.package-archive", Path.GetFileName(path));
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return null;
            }
        }


        [HttpGet]
        public IActionResult IOS() => View();


        [HttpGet]
        public IActionResult ContactUs([FromServices] IUserService userService)
        {
            if (User.Identity.IsAuthenticated)
                return View(new ContactUsDTO
                {
                    FullName = User.GetFullname(),
                    MobileNumber = long.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value)
                });
            return View(new ContactUsDTO());
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs([FromServices] IContactUsService contactUsService, ContactUs model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            if (User.Identity.IsAuthenticated) model.UserId = User.GetUserId();
            return Json(await contactUsService.AddAsync(model));
        }

        [HttpGet]
        public IActionResult AboutUs() => View();


        [HttpGet]
        public IActionResult Rules() => View();
    }
}
