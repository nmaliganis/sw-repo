using sw.localization.api.Configurations;
using sw.localization.api.Configurations.Installers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace sw.localization.api;

/// <summary>
/// Class: Startup
/// </summary>
public class Startup
{
    /// <summary>
    /// Constructor : Startup
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="hostEnv"></param>
    public Startup(IConfiguration configuration, IWebHostEnvironment hostEnv)
    {
        Configuration = configuration;
        HostEnv = hostEnv;
    }

    /// <summary>
    /// Property : Configuration
    /// </summary>
    public IConfiguration Configuration { get; }
    /// <summary>
    /// Property : HostEnv
    /// </summary>
    public IWebHostEnvironment HostEnv { get; }

    /// <summary>
    /// Method : ConfigureServices
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();
        services.AddAspBasicInstaller();

        services.AddSwaggerInstaller();

        services.AddSerilogInstaller(Configuration, HostEnv);

        services.AddAuthInstaller(Configuration);

        services.AddRoutingVersioningInstaller();

        services.AddAutoMapperInstaller();

        services.ConfigureInfrastructure();
        services.ConfigureContracts();
        services.ConfigureEfCoreWithNpgsql(Configuration.GetConnectionString("PostgreSqlDatabase"));

        services.AddMediatR(typeof(Startup));

        services.AddNpgSqlHealthCheckInstaller(Configuration);

        services.AddListStartupServicesInstaller();
    }
    /// <summary>
    /// Method : Configure
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsStaging() || env.IsProduction())
        {
            app.UseListStartupServicesInstaller();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseRouting();
        app.UseAuthInstaller();
        app.UseAspBasicInstaller();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        app.UseHealthCheckInstaller();
        app.UseSwaggerInstaller();
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