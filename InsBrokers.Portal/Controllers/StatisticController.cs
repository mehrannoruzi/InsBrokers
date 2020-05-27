using Elk.AspNetCore;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InsBrokers.Portal.Controllers
{
    //[AuthorizationFilter]
    public class StatisticController : Controller
    {
        private readonly ILossService _lossSrv;
        private readonly IUserService _userSrv;
        public StatisticController(ILossService lossSrv, IUserService userSrv)
        {
            _lossSrv = lossSrv;
            _userSrv = userSrv;
        }

        public async Task<IActionResult> Index()
        {
            var getUserCount = await _userSrv.GetUserCount();
            if (!getUserCount.IsSuccessful) return RedirectToAction("Index", "Error", new { code = 500 });
            var getLossCount = await _lossSrv.GetLossCount();
            if (!getLossCount.IsSuccessful) return RedirectToAction("Index", "Error", new { code = 500 });
            var userInDays = await _userSrv.GetUserCountLastDaysAsync(7);
            if (!userInDays.IsSuccessful) return RedirectToAction("Index", "Error", new { code = 500 });
            var lossInDays = await _lossSrv.GetLossCountLastDaysAsync(7);
            if (!lossInDays.IsSuccessful) return RedirectToAction("Index", "Error", new { code = 500 });
            return View(new StatisticModel
            {
                UserCount = getUserCount.Result,
                LossCount = getLossCount.Result,
                UserInDays = userInDays.Result,
                LossInDays = lossInDays.Result
            });
        }
    }
}
