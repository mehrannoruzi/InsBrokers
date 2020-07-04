using Elk.Core;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InsBrokers.Portal.Controllers
{
    //[AuthorizationFilter]
    public partial class MemberContactUsController : Controller
    {
        private readonly IContactUsService _ContactUsSrv;
        private readonly IUserService _userSrv;

        public MemberContactUsController(IContactUsService ContactUsSrv, IUserService userSrv)
        {
            _ContactUsSrv = ContactUsSrv;
            _userSrv = userSrv;
        }

        //[HttpGet, AuthorizationFilter]
        public async Task<IActionResult> Index()
        {
            var user = await _userSrv.FindAsync(User.GetUserId());
            return View(new ContactUsDTO
            {
                FullName = user.Result.Fullname,
                MobileNumber = user.Result.MobileNumber
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Add(ContactUs model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.UserId = User.GetUserId();
            return Json(await _ContactUsSrv.AddAsync(model));
        }

    }
}