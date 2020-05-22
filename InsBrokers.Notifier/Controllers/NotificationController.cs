using InsBrokers.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Notifier.Service;
using InsBrokers.Notifier.Filters;

namespace InsBrokers.Notifier.Controllers
{
    public class NotificationController : Controller
    {
        public INotificationService _notificationService { get; }

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpGet]
        public IActionResult Index()
            => Ok("WellCome To InsBrokers.Notifier Api ...");

        [HttpPost, AuthenticationFilter]
        public async Task<IActionResult> AddAsync([FromBody] NotificationDto notifyDto, Application application)
            => Ok(await _notificationService.AddAsync(notifyDto, application.ApplicationId));

    }
}