using sw.routing.api.Configurations;
using sw.routing.api.Configurations.Installers;
using sw.routing.model.CustomTypes;
using sw.routing.services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace sw.routing.api;

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

        services.ConfigureInfrastructure();

        services.ConfigureContracts();

        services.ConfigureNHibernateWithNpgsql(this.Configuration.GetConnectionString("PostgreSqlDatabase"));

        services.AddMediatR(typeof(ItineraryServicesMarker).Assembly);

        services.AddNpgSqlHealthCheckInstaller(this.Configuration);

        services.AddApplicationInsights(Configuration);

        services.AddCustomMvc();

        services.AddListStartupServicesInstaller();

        services.RegisterAsbAsConsumerServices(this.Configuration);
        services.AddAbsHealthCheckInstaller(this.Configuration);
        services.AddHttpClient();
        services.AddControllers().AddDapr();

        NpgsqlConnection.GlobalTypeMapper.UseJsonNet(
            new[] { typeof(ConfigJsonbType), typeof(ZoneJsonbType), typeof(OccurrenceJsonbType) }
        );

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
        app.UseAuthInstaller();
        app.UseAspBasicInstaller();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        app.UseHealthCheckInstaller();
        app.UseRoutingVersioningInstaller();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }

} // Class : Startup