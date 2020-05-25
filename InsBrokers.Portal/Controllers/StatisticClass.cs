using System;
using Elk.Http;
using Elk.Core;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InsBrokers.Portal.Resource;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{
    public class StatisticController : Controller
    {
        private readonly ILossService lossSrv;
        public StatisticController(ILossService lossSrv)
        {

        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
