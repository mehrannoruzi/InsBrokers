using Quartz;
using Elk.Core;
using Quartz.Spi;
using Quartz.Impl;
using InsBrokers.Domain;
using Elk.EntityFrameworkCore;
using InsBrokers.Notifier.Service;
using Microsoft.EntityFrameworkCore;
using InsBrokers.Notifier.DataAccess.Ef;
using Microsoft.Extensions.Configuration;
using InsBrokers.Notifier.DataAccess.Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace InsBrokers.Notifier.DependencyResolver
{
    public static class NotifierDiExtension
    {
        public static IServiceCollection AddTransient(this IServiceCollection serviceCollection, IConfiguration _configuration)
        {
            //serviceCollection.AddTransient<IUserRepo, UserRepo>();

            return serviceCollection;
        }

        public static IServiceCollection AddScoped(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddContext<NotifierDbContext>(_configuration.GetConnectionString("NotifierDbContext"));

            services.AddScoped<NotifierUnitOfWork, NotifierUnitOfWork>();

            services.AddScoped(typeof(IGenericRepo<>), typeof(EfGenericRepo<>));


            #region Notifier

            services.AddScoped<IEventMapperRepo, EventMapperRepo>();
            services.AddScoped<IApplicationRepo, ApplicationRepo>();
            services.AddScoped<INotificationRepo, NotificationRepo>();

            services.AddScoped<INotificationService, NotificationService>();

            #endregion

            return services;
        }

        public static IServiceCollection AddSingleton(this IServiceCollection services, IConfiguration _configuration)
        {
            //services.AddSingleton<IMemoryCacheProvider, MemoryCacheProvider>();
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<NotificationProcess>();
            services.AddSingleton(new JobSchedule(
                                    jobType: typeof(NotificationProcess),
                                    cronExpression: _configuration["NotifierSettings:NotificationProcessPattern"]));


            services.AddSingleton<NotifierSetting>(new NotifierSetting(
                _configuration["NotifierSettings:NotifierUrl"],
                _configuration["NotifierSettings:NotificationProcessPattern"],
                _configuration["NotifierSettings:EmailServiceConfig:EmailHost"],
                _configuration["NotifierSettings:EmailServiceConfig:EmailUserName"],
                _configuration["NotifierSettings:EmailServiceConfig:EmailPassword"]));

            return services;
        }

        public static IServiceCollection AddContext<TDbContext>(this IServiceCollection serviceCollection, string conectionString) where TDbContext : DbContext
        {
            serviceCollection.AddDbContext<TDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(conectionString,
                            sqlServerOption =>
                            {
                                sqlServerOption.MaxBatchSize(1000);
                                sqlServerOption.CommandTimeout(null);
                                sqlServerOption.UseRelationalNulls(false);
                                //sqlServerOption.EnableRetryOnFailure();
                                //sqlServerOption.UseRowNumberForPaging(false);
                            });
                //.AddInterceptors(new DbContextInterceptors());
            });

            return serviceCollection;
        }
    }
}