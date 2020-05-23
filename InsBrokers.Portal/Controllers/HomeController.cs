using Elk.Core;
using InsBrokers.Domain;
using InsBrokers.Portal.Resource;
using InsBrokers.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

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

        public IActionResult Auth() => View();

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

            return Json(new {IsSuccessful = true, Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { })});
        }
    }
}
