using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public class CreateEmailStrategy : ICreateStrategy
    {
        public Task Create(NotificationDto notifyDto, INotificationRepo notificationRepo, int applicationId)
        {
            var notification = new Notification
            {
                TryCount = 0,
                Type = NotificationType.Email,
                Status = NotificationStatus.Insert,

                ApplicationId = applicationId,
                ExtraData = notifyDto.UserId.ToString(),
                Content = notifyDto.Content,
                FullName = notifyDto.FullName,
                Receiver = notifyDto.Email
            };
            notificationRepo.AddAsync(notification);

            return Task.CompletedTask;
        }
    }
}