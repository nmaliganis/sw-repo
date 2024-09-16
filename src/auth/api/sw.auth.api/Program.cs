using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.Core;
using System;
using Azure.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace sw.auth.api;

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

        return builder.Build();
    }

    /// <summary>
    /// Main
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var configuration = GetConfiguration();
        NHibernateProfilerBootstrapper.PreStart();
        CreateWebHostBuilder(configuration, args).Build().Run();
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
            .ConfigureAppConfiguration(x => 
                x.AddConfiguration(configuration)
                    .AddCommandLine(args)
                )
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseUrls(urls: "http://0.0.0.0:5200")
            .UseSerilog()
        ;
} // Class : Program