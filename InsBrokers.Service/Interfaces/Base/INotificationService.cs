using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Service
{
    public interface INotificationService
    {
        Task<bool> NotifyAsync(NotificationDto notifyDto);
    }
}
