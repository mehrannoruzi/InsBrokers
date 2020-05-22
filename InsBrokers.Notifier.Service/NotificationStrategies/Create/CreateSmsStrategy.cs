using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public class CreateSmsStrategy : ICreateStrategy
    {
        public Task Create(NotificationDto notifyDto, INotificationRepo notificationRepo, int applicationId)
        {
            var notification = new Notification { 
                TryCount = 0,
                Type = NotificationType.Sms,
                Status = NotificationStatus.Insert,

                ApplicationId = applicationId,
                ExtraData = notifyDto.UserId.ToString(),
                Content = notifyDto.Content,
                FullName = notifyDto.FullName,
                Receiver = notifyDto.MobileNumber.ToString()
            };
            notificationRepo.AddAsync(notification);

            return Task.CompletedTask;
        }
    }
}