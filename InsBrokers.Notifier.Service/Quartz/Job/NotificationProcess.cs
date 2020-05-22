using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InsBrokers.Notifier.Service
{
    public class NotificationProcess : IJob
    {
        private IServiceProvider _serviceProvider { get; }

        public NotificationProcess(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var notificationService = scope.ServiceProvider.GetService<INotificationService>();
                notificationService.SendAsync();
            }

            return Task.CompletedTask;
        }
    }
}
