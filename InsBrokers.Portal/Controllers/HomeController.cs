using Elk.Core;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InsBrokers.Portal.Resource;
using Microsoft.Extensions.Configuration;
using Elk.AspNetCore;
using System.Linq;
using System.Security.Claims;

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
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]PortalSignUpModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { Message = Strings.ValidationFailed });
            model.MemberRoleId = int.Parse(_configuration["CustomSettings:MemberRoleId"]);
            var save = await _userSrv.SignUp(model);
            if (!save.IsSuccessful) return Json(save);
            var menuRep = _userSrv.GetAvailableActions(save.Result.UserId, null, _configuration["CustomSettings:UrlPrefix"]);
            if (menuRep == null) return Json(new Response<string> { IsSuccessful = false, Message = Strings.ThereIsNoViewForUser });

            await CreateCookie(save.Result, true);

            return Json(new { IsSuccessful = true, Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { }) });
        }

        [HttpGet]
        public IActionResult Android() => View();


        [HttpGet]
        public IActionResult IOS() => View();


        [HttpGet]
        public IActionResult ContactUs([FromServices]IUserService userService)
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
        public async Task<IActionResult> ContactUs([FromServices]IContactUsService contactUsService, ContactUs model)
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
