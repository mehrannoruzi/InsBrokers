using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public interface ISendStrategy
    {
        Task SendAsync(Notification notification, INotificationRepo notificationRepo);
    }
}