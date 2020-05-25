using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace InsBrokers.Portal.Controllers
{
    public class ErrorController : Controller
    {

        public IActionResult Index(int code)
        {
            //var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            //if(context.Exception is HttpResponseException exception)
            //switch (statusCode)
            //{
            //    case 404:
            //        ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
            //        //ViewBag.RouteOfException = statusCodeData.OriginalPath;
            //        break;
            //    case 500:
            //        ViewBag.ErrorMessage = "Sorry something went wrong on the server";
            //        //ViewBag.RouteOfException = statusCodeData.OriginalPath;
            //        break;
            //}

            return View(code);
        }
    }
}
