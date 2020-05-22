using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public interface ICreateStrategy
    {
        Task Create(NotificationDto notifyDto, INotificationRepo notificationRepo, int applicationId);
    }
}