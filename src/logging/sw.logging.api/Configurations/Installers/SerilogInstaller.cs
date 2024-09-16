using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace sw.logging.api.Configurations.Installers;

internal static class SerilogInstaller
{
    public static IServiceCollection AddSerilogInstaller(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment hostEnv)
    {
        var name = Assembly.GetExecutingAssembly().GetName();
        var seqServerUrl = configuration["Serilog:SeqServerUrl"];
        var logstashUrl = configuration["Serilog:LogstashgUrl"];

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Debug()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("sw.logging.api", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
            .WriteTo.DurableHttpUsingTimeRolledBuffers(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
            .ReadFrom.Configuration(configuration)
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{name.Name}")
            .Enrich.WithProperty("Revision", $"{name.Version}")
            .WriteTo.Debug(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{HttpContext} {NewLine}{Exception}")
            .WriteTo.File("logs.txt", Serilog.Events.LogEventLevel.Information,
                retainedFileCountLimit: 7)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
            loggingBuilder
                .AddSerilog(dispose: true));

        return services;
    }
}