using Elk.Core;
using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public interface INotificationService
    {
        Task<IResponse<bool>> AddAsync(NotificationDto notifyDto, int applicationId);
        
        Task SendAsync();
    }
}
