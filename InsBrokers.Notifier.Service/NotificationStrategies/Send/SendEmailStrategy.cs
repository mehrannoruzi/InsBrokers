using System;
using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Notifier.Service
{
    public class SendEmailStrategy : ISendStrategy
    {
        public Task SendAsync(Notification notification, INotificationRepo notifierUnitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}