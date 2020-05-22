using System;
using InsBrokers.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace InsBrokers.Notifier.DataAccess.Dapper
{
    public sealed class NotifierUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        public NotifierUnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public IEventMapperRepo EventMapperRepo => _serviceProvider.GetService<IEventMapperRepo>();
        public IApplicationRepo ApplicationRepo => _serviceProvider.GetService<IApplicationRepo>();
        public INotificationRepo NotificationRepo => _serviceProvider.GetService<INotificationRepo>();
    }
}