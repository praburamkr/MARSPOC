using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationWebApi.Configuration;
using GL.MARS.CommunicationBlocks.AZServiceBus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GL.MARS.CommunicationBlocks.EventBus.Concepts;
using GL.MARS.CommunicationBlocks.EventBus;
using GL.MARS.Models.NotificationEvents;
using NotificationWebApi;
using Common.Interfaces;
using Common.Utility;
using Notification.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using Notification.Repositories;

namespace Notification
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://*");
            }));

            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<UserRepository>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.Configure<NotificationHubConfiguration>(Configuration.GetSection("NotificationHub"));

            services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            //services.AddMvc();

            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ILogHandler, TelemetryLogHandler>();

            //NTOE: May need to uncomment when we use Event Bus for Async communication
            // Registering Azure service bus Topic...
            //services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            //{
            //    var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

            //    var serviceBusConnectionString = Configuration["EventBusConnection"];
            //    var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

            //    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
            //});
            // Registering Azure service bus Subscription...
            //RegisterEventBus(services);

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<WebNotificationHub>("/api/notifications/web");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
            //NTOE: May need to uncomment when we use Event Bus for Async communication
            //ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<PushNotificationIntegrationEvent, NotificationEventHandler>();

        }

        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];


            services.AddSingleton<IEventBus, AZServiceBus>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<AZServiceBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new AZServiceBus(serviceBusPersisterConnection, logger,
                    eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<NotificationEventHandler>();
        }
    }
}
