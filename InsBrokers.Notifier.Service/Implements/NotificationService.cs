using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using System.Threading.Tasks;
using InsBrokers.Notifier.Service.Resource;
using InsBrokers.Notifier.DataAccess.Dapper;

namespace InsBrokers.Notifier.Service
{
    public class NotificationService : INotificationService
    {
        private NotifierUnitOfWork _notifierUnitOfWork;

        public NotificationService(NotifierUnitOfWork notifierUnitOfWork)
        {
            _notifierUnitOfWork = notifierUnitOfWork;
        }



        public async Task<IResponse<bool>> AddAsync(NotificationDto notifyDto, int applicationId)
        {
            try
            {
                if (notifyDto == null || string.IsNullOrWhiteSpace(notifyDto.Content)) return new Response<bool> { IsSuccessful = false, Message = ServiceMessage.InvalidParameter };

                var eventMappers = await _notifierUnitOfWork.EventMapperRepo.GetAsync(notifyDto.Type, applicationId);
                if (!eventMappers.Any()) return new Response<bool> { Message = ServiceMessage.EventNotExist };

                var notifRepo = _notifierUnitOfWork.NotificationRepo;
                foreach (var notifyStrategy in eventMappers)
                    await CreateNotificationFactory.GetStrategy(notifyStrategy.NotifyStrategy).Create(notifyDto, notifRepo, applicationId);

                return new Response<bool> { IsSuccessful = true, Result = true, Message = ServiceMessage.Success };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<bool> { Message = ServiceMessage.Exception };
            }
        }

        public async Task SendAsync()
        {
            try
            {
                var notifRepo = _notifierUnitOfWork.NotificationRepo;
                var notifications = await notifRepo.GetUnProccessAsync();
                if (!notifications.Any()) return;

                foreach (var notif in notifications)
                    await SendNotificationFactory.GetStrategy(notif.Type).SendAsync(notif, notifRepo);
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
            }
        }
    }
}
