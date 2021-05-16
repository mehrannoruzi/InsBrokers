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

namespace InsBrokers.Portal.Controllers
{
    public class HomeController : BaseAuthController
    {
        private readonly IUserService _userSrv;
        private IConfiguration _configuration;

        public HomeController(IUserService userSrv, IConfiguration configuration, IHttpContextAccessor httpAccessor) : base(httpAccessor)
        {
            _userSrv = userSrv;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var organizationList = new List<SelectListItem>
            {
                new SelectListItem { Text="", Value="", Selected = true },
                new SelectListItem { Text="سازمان نظام دانپزشکی", Value="سازمان نظام دانپزشکی", Selected = false },
            };

            var insurancePlanList = new List<SelectListItem>
            {
                new SelectListItem { Text="", Value="", Selected = true },
                new SelectListItem { Text="طرح برنزی", Value="طرح برنزی", Selected = false },
                new SelectListItem { Text="طرح نقره ای", Value="طرح نقره ای", Selected = false },
                new SelectListItem { Text="طرح طلایی", Value="طرح طلایی", Selected = false },
            };

            ViewBag.OrganizationList = organizationList;
            ViewBag.InsurancePlan = insurancePlanList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] PortalSignUpModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { Message = ModelState.GetModelError() });

            model.MemberRoleId = int.Parse(_configuration["CustomSettings:MemberRoleId"]);
            var save = await _userSrv.SignUp(model);
            if (!save.IsSuccessful) return Json(save);

            var menuRep = _userSrv.GetAvailableActions(save.Result.UserId, null, _configuration["CustomSettings:UrlPrefix"]);
            if (menuRep == null) return Json(new Response<string> { IsSuccessful = false, Message = Strings.ThereIsNoViewForUser });

            await CreateCookie(save.Result, true);

            return Json(new { IsSuccessful = true, Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { }) });
        }

        [HttpGet]
        public async Task<FileResult> Android([FromServices] IWebHostEnvironment env)
        {
            var path = Path.Combine(env.WebRootPath, "Files", "DarmanNaft.apk");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.android.package-archive", Path.GetFileName(path));
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
