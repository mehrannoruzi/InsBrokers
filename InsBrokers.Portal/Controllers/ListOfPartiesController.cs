using Microsoft.AspNetCore.Mvc;

namespace InsBrokers.Portal.Controllers
{
    public class ListOfPartiesController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}