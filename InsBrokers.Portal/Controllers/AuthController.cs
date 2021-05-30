using Elk.Core;
using Elk.Cache;
using Elk.AspNetCore;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.DataAccess.Ef;
using InsBrokers.InfraStructure;
using Microsoft.AspNetCore.Http;
using InsBrokers.Portal.Resource;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace InsBrokers.Portal.Controllers
{
    public partial class AuthController : BaseAuthController
    {
        private readonly IUserService _userSrv;
        private IConfiguration _config { get; set; }
        private readonly IHttpContextAccessor _httpAccessor;
        private const string UrlPrefixKey = "CustomSettings:UrlPrefix";

        private readonly AuthDbContext _db;
        private readonly AppDbContext _appDb;

        public AuthController(IHttpContextAccessor httpAccessor, IConfiguration configuration,
            IUserService userSrv, AuthDbContext db, AppDbContext appDb) : base(httpAccessor)
        {
            _userSrv = userSrv;
            _config = configuration;
            _httpAccessor = httpAccessor;
            _db = db;
            _appDb = appDb;
        }


        [HttpGet]
        public virtual ActionResult SignIn()
        {
            //var t = new AclSeed(_db,_appDb);
            //var rep = t.Init();
            //var pw = HashGenerator.Hash("9334188184");
            if (User.Identity.IsAuthenticated)
            {
                var urlPrefix = _config.GetValue<string>(UrlPrefixKey);
                var defaultUA = _userSrv.GetAvailableActions(User.GetUserId(), null, urlPrefix).DefaultUserAction;
                return Redirect($"{urlPrefix}/{defaultUA.Controller}/{defaultUA.Action}");
            }
            return View(new SignInModel { RememberMe = true });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> SignIn([FromForm] SignInModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });

            var chkRep = await _userSrv.Authenticate(long.Parse(model.MobileNumber), model.Password);
            if (!chkRep.IsSuccessful) return Json(new Response<string> { IsSuccessful = false, Message = chkRep.Message });

            var menuRep = _userSrv.GetAvailableActions(chkRep.Result.UserId, null, _config.GetValue<string>(UrlPrefixKey));
            if (menuRep == null) return Json(new Response<string> { IsSuccessful = false, Message = Strings.ThereIsNoViewForUser });

            await CreateCookie(chkRep.Result, model.RememberMe);
            return Json(new Response<string> { IsSuccessful = true, Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { }), });
        }

        public virtual async Task<ActionResult> SignOut([FromServices] IMemoryCacheProvider cacheProvider)
        {
            if (User.Identity.IsAuthenticated)
            {
                cacheProvider.Remove(GlobalVariables.CacheSettings.MenuModelCacheKey(User.GetUserId()));
                cacheProvider.Remove(GlobalVariables.CacheSettings.MainMenuCacheKey(User.GetUserId()));
                await _httpAccessor.HttpContext.SignOutAsync();
            }

            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public virtual ActionResult RecoverPasswrod() => View();

        [HttpPost]
        public virtual async Task<JsonResult> RecoverPasswrod(string mobileNumber)
        {
            var emailModel = new EmailMessage();
            emailModel.Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_NewPassword", "");
            return Json(await _userSrv.RecoverPassword(long.Parse(mobileNumber), _config["CustomSettings:EmailServiceConfig:EmailUserName"], emailModel));
        }

    }
}