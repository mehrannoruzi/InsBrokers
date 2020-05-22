using System;
using Quartz;
using Quartz.Spi;
using Microsoft.Extensions.DependencyInjection;

namespace InsBrokers.Notifier.Service
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            => _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;

        public void ReturnJob(IJob job)
        {
            //if (job is IDisposable disposableJob) disposableJob.Dispose();
        }
    }
}