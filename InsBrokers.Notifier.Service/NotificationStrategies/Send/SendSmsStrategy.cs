using System;
using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public class SendSmsStrategy : ISendStrategy
    {
        public async Task SendAsync(Notification notification, INotificationRepo notificationRepo)
        {
            var sendResult = await LinePayamakProvider.SendSmsAsync(notification.Receiver, notification.Content);

            var updateModel = new UpdateNotificationDto
            {
                NotificationId = notification.NotificationId,
                SendDateMi = DateTime.Now,
                SendStatus = sendResult
            };

            if (sendResult.Split(':')[1] == "1")
            {
                updateModel.Status = NotificationStatus.Success;
                updateModel.IsLock = true;
            }
            else
            {
                updateModel.Status = NotificationStatus.Failed;
                updateModel.IsLock = false;
            }

            await notificationRepo.UpdateAsync(updateModel);
        }
    }
}