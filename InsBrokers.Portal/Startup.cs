using Elk.Core;
using Elk.Http;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using InsBrokers.DependencyResolver;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InsBrokers.Portal
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
            services.AddControllersWithViews()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = null;
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                    {
                        option.Cookie.SameSite = SameSiteMode.Lax;
                    });

            services.AddHttpContextAccessor();

            services.AddTransient(_config);
            services.AddScoped(_config);
            services.AddSingleton(_config);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cachePeriod = "0";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); app.UseStaticFiles();
                app.UseStaticFiles();
                cachePeriod = "1";
            }
            else
            {
                cachePeriod = "604800";
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = ctx => { ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}"); }
                });

                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();

                app.Use(async (context, next) =>
                {
                    await next.Invoke();
                    if (!context.Request.IsAjaxRequest())
                    {
                        var handled = context.Features.Get<IStatusCodeReExecuteFeature>();
                        var exp = context.Features.Get<IExceptionHandlerFeature>();
                        var statusCode = context.Response.StatusCode;
                        if (handled == null && statusCode >= 400)
                        {
                            FileLoger.Info(exp.Error.Message);
                            context.Response.Redirect($"/Error/Index?code={statusCode}");
                        }
                    }

                });
            }
            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Register}");
                });
        }
    }
}