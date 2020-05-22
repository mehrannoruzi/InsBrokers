using Quartz;
using Quartz.Spi;
using System.Threading;
using System.Threading.Tasks;
using InsBrokers.Notifier.Service;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace InsBrokers.Notifier.QuartzService
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;

        public IScheduler Scheduler { get; set; }

        public QuartzHostedService(ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory, IEnumerable<JobSchedule> jobSchedules)
        {
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
            _schedulerFactory = schedulerFactory;
        }


        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                //.StartAt(DateTimeOffset.Now.AddSeconds(25))
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
            => await Scheduler?.Shutdown(cancellationToken);
    }
}
