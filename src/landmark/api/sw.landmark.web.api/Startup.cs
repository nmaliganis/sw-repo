using sw.landmark.api.Configurations;
using sw.landmark.api.Proxies;
using sw.landmark.services;
using sw.landmark.web.api.Configurations.Installers;
using sw.auth.messaging.Providers.AzureServiceBus;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace sw.landmark.api
{
    /// <summary>
    /// Startup class for configurations
    /// </summary>
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

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddAspBasicInstaller();

            services.AddSwaggerInstaller();

            services.AddSerilogInstaller(this.Configuration, this.HostEnv);

            services.AddAuthInstaller(this.Configuration);

            services.AddRoutingVersioningInstaller();

            services.AddAutoMapperInstaller();

            services.CongifureInfrastructure();
            services.ConfigureContracts();
            services.ConfigureEfCoreWithNpgsql(this.Configuration.GetConnectionString("PostgreSqlDatabase"));

            services.AddMediatR(typeof(LandmarkServicesMarker).Assembly);

            services.AddNpgSqlHealthCheckInstaller(this.Configuration);

            services.AddListStartupServicesInstaller();

            services.RegisterAzureServiceBusAsConsumerServices(this.Configuration);

            services.AddSingleton<IMemberRegisteredProxyManipulator, MemberRegisteredProxyManipulator>();
        }

        /// <summary>
        /// Configure application builder
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging() || env.IsProduction())
            {
                app.UseListStartupServicesInstaller();
                app.UseDeveloperExceptionPage();
                app.UseSwaggerInstaller();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseAspBasicInstaller();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });

            app.UseHealthCheckInstaller();
            app.UseSwaggerInstaller();
            app.UseRoutingVersioningInstaller();

            app.UseAuthInstaller();

            #region Proxies

            var serviceProvider = app.ApplicationServices;
            var serviceMemberRegisterProxy = (IMemberRegisteredProxyManipulator)serviceProvider.GetService(typeof(IMemberRegisteredProxyManipulator));
            serviceMemberRegisterProxy?.ProxyInitializer();

            #endregion

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