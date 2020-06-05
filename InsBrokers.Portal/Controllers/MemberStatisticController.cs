using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elk.Core;
using InsBrokers.Service;
using Microsoft.AspNetCore.Mvc;

namespace InsBrokers.Portal.Controllers
{
    public class MemberStatisticController : Controller
    {
        private readonly IUserService _userSrv;
        public MemberStatisticController(IUserService userSrv)
        {
            _userSrv = userSrv;
        }
        public IActionResult Index()
        {
            return View(_userSrv.GetMainMenu(User.GetUserId()));
        }
    }
}