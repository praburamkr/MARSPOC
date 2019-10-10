using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointment.Models;
using Appointment.Repositories;
using Common.Interfaces;
using Common.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Appointment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ILogHandler, TelemetryLogHandler>();

            services.AddDbContext<AppointmentTypeRepository>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<ReasonCodeRepository>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<AppointmentRepository>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.AddScoped<IAvailabilitySearch, AvailabilitySearchRepository>();
            services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("AllowAnyOrigin");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
