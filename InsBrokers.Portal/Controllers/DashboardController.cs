using Elk.AspNetCore;
using Elk.Core;
using InsBrokers.Domain;
using InsBrokers.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InsBrokers.Portal.Controllers
{
    public class DashboardController : Controller
    {
        IUserService _userSrv;
        public DashboardController(IUserService userSrv)
        {
            _userSrv = userSrv;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, AuthorizationFilter]
        public async Task<IActionResult> ContactUs()
        {
            var user = await _userSrv.FindAsync(User.GetUserId());
            return View(new ContactUsDTO
            {
                FullName = user.Result.Fullname,
                MobileNumber = user.Result.MobileNumber
            });
        }

        [HttpGet, AuthorizationFilter]
        public virtual IActionResult FanClub() => View();
    }
}