using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public class CreateTeleBotStrategy : ICreateStrategy
    {
        public Task Create(NotificationDto notifyDto, INotificationRepo notificationRepo, int applicationId)
        {
            var notification = new Notification
            {
                TryCount = 0,
                Type = NotificationType.TeleBot,
                Status = NotificationStatus.Insert,

                ApplicationId = applicationId,
                ExtraData = notifyDto.UserId.ToString(),
                Content = notifyDto.Content,
                FullName = notifyDto.FullName,
                Receiver = notifyDto.TelegramChatId
            };
            notificationRepo.AddAsync(notification);

            return Task.CompletedTask;
        }
    }
}