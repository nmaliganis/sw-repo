using sw.auth.api.Configurations;
using sw.auth.api.Configurations.Installers;
using sw.auth.api.Proxies.DriverCreator;
using sw.auth.services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace sw.auth.api;

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
        services.AddAspBasicInstaller();

        services.AddSwaggerInstaller();

        services.AddSerilogInstaller(this.Configuration, this.HostEnv);

        services.AddAuthInstaller(this.Configuration);

        services.AddRoutingVersioningInstaller();

        services.AddAutoMapperInstaller();

        services.ConfigureValidators();

        services.ConfigureInfrastructure();

        services.ConfigureContracts();

        services.ConfigureNHibernateWithNpgsql(this.Configuration.GetConnectionString("PostgreSqlDatabase"));

        services.AddMediatR(typeof(AuthServicesMarker).Assembly);

        services.AddNpgSqlHealthCheckInstaller(this.Configuration);

        services.AddCustomMvc();

        services.AddListStartupServicesInstaller();

        services.RegisterAsbAsPublisherServices(this.Configuration);
        services.AddAbsHealthCheckInstaller(this.Configuration);
        services.AddHttpClient();

        services.AddSingleton<IDriverCreatorProxyManipulator, DriverCreatorProxyManipulator>();

        services.AddControllers()
            .AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

        var configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
            .AddEnvironmentVariables()
            .Build();

        app.UseRouting();

        app.UseAuthInstaller();
        app.UseAspBasicInstaller();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        app.UseCloudEvents();

        app.UseHealthCheckInstaller();
        app.UseRoutingVersioningInstaller();

        #region Proxies

        var serviceProvider = app.ApplicationServices;
        var serviceMemberRegisterProxy = (IDriverCreatorProxyManipulator)serviceProvider.GetService(typeof(IDriverCreatorProxyManipulator));
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
} // Class : Startup