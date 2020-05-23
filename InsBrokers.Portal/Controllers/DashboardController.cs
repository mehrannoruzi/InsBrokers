using Microsoft.AspNetCore.Mvc;

namespace InsBrokers.Portal.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}