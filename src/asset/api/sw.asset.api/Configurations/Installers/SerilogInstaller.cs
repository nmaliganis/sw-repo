﻿using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace sw.asset.api.Configurations.Installers;

internal static class SerilogInstaller
{
    public static IServiceCollection AddSerilogInstaller(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment hostEnv)
    {
        var name = Assembly.GetExecutingAssembly().GetName();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Debug()
            .MinimumLevel.Override("sw.asset.api", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{name.Name}")
            .Enrich.WithProperty("Revision", $"{name.Version}")
            .WriteTo.Debug(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{HttpContext} {NewLine}{Exception}")
            .WriteTo.File($"logs.txt", Serilog.Events.LogEventLevel.Information,
                retainedFileCountLimit: 7)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
            loggingBuilder
                .AddSerilog(dispose: true));

        return services;
    }
}