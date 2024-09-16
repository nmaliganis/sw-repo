using System;
using System.IO;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace sw.routing.api;

/// <summary>
/// Class : Program
/// </summary>
public class Program
{
    static IConfiguration GetConfiguration() {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        return builder
                .Build()
            ;
    }

    /// <summary>
    /// Main
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var configuration = GetConfiguration();
        NHibernateProfilerBootstrapper.PreStart();
        CreateWebHostBuilder(configuration, args)
            .Build()
            .Run()
            ;
    }

    /// <summary>
    /// CreateWebHostBuilder
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IWebHostBuilder CreateWebHostBuilder(IConfiguration configuration, string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(true)
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseUrls(urls: "http://0.0.0.0:5600")
            .UseSerilog()
        ;
} // Class : Program