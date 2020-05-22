using System;
using Elk.AspNetCore;
using InsBrokers.Notifier.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using InsBrokers.Notifier.QuartzService;
using Microsoft.Extensions.Configuration;
using InsBrokers.Notifier.DependencyResolver;
using Microsoft.Extensions.DependencyInjection;

namespace InsBrokers.Notifier
{
    public class Startup
    {
        private IConfiguration _config { get; }

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                option.ReturnHttpNotAcceptable = true;
                // option.Filters.Add(typeof(ModelValidationFilter));
            })
            .AddXmlSerializerFormatters()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            //services.AddMemoryCache();

            services.AddTransient(_config);
            services.AddScoped(_config);
            services.AddSingleton(_config);

            services.AddHostedService<QuartzHostedService>();
            TelegramBot.Initialize(_config["NotifierSettings:TelegramBotToken"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();
            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        var errorhandler = context.Features.Get<IExceptionHandlerPathFeature>();
            //        context.Response.StatusCode = 500;
            //        context.Response.ContentType = "application/Json";
            //        var bytes = System.Text.Encoding.ASCII.GetBytes(new { IsSuccessful = false, errorhandler.Error?.Message }.ToString());
            //        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            //    });
            //});

            app.UseAuthorization();

            app.UseMvc(routeConfig =>
            {
                routeConfig.MapRoute("Default", "{controller=Notification}/{action=Index}");
            });
        }
    }
}