using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Controllers;
using Authentication.Models;
using Authentication.Repositories;
using Common.Interfaces;
using Common.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication
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

            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<ImageHandlerRepository>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IOAuthToken, OAuthToken>();
            services.AddScoped<IUserAuthentication, UserAuthentication>();
            services.AddScoped<IJWTToken, JWTToken>();
            services.AddScoped<IUserCredAuthentication, UserCredAuthentication>();
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

            //app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseCors("AllowAnyOrigin");
            app.UseMvc();
        }
    }
}
