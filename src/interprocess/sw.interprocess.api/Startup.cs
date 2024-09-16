using sw.interprocess.api.Helpers;
using sw.interprocess.api.Helpers.Infrastructure;
using sw.interprocess.api.Installers;
using sw.interprocess.api.Mqtt;
using sw.interprocess.api.Schedulers;
using sw.interprocess.api.WSs;
using sw.azure.messaging.Providers.AzureServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.IO;

namespace sw.interprocess.api
{
    public class Startup
    {
        /// <summary>
        /// Startup ctor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostEnv"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnv)
        {
            this.Configuration = configuration;
            this.HostEnv = hostEnv;
        }

        /// <summary>
        /// IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// IWebHostEnvironment
        /// </summary>
        public IWebHostEnvironment HostEnv { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAspBasicInstaller();
            services.AddSerilogInstaller(this.Configuration, this.HostEnv);
            services.AddRoutingVersioningInstaller();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "sw.interprocess.api - HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API for sw.interprocess.api service",
                });
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description =
              "Authorization: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
              {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            new string[] { }
          }
              });
            });

            services.AddControllers(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); })
              // Added for functional tests
              .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
            builder => builder
              .SetIsOriginAllowed((host) => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
            });
            services.AddSingleton<IWsConfiguration, WsConfiguration>();
            services.AddSingleton<IMqttConfiguration, MqttConfiguration>();

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<KeepAliveWsInitializerJob>();

            services.AddSingleton(new JobSchedule(
              jobType: typeof(KeepAliveWsInitializerJob),
              cronExpression: "0/5 * * * * ?")); // run every 5 seconds

            var configAIoT = new ConfigAIoT(new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build());

            services.AddSingleton<IConfigAIoT>(configAIoT);

            services.RegisterAsbAsPublisherServices(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging() || env.IsProduction())
            {
                app.UseListStartupServicesInstaller();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "sw.interprocess.api v1"));
            }
            else
            {
                app.UseHsts();
            }

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddEnvironmentVariables()
                .Build();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAspBasicInstaller();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });


            //#region Proxies
            var serviceProvider = app.ApplicationServices;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var serviceWsProxy = (IWsConfiguration)serviceProvider.GetService(typeof(IWsConfiguration));
            serviceWsProxy?.EstablishConnection();

            var serviceMqttProxy = (IMqttConfiguration)serviceProvider.GetService(typeof(IMqttConfiguration));
            serviceMqttProxy?.EstablishConnection();

            //#endregion

            app.UseCookiePolicy();
            app.UseHttpsRedirection();

            app.UseRoutingVersioningInstaller();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}