using Ardalis.ListStartupServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace sw.asset.api.Configurations.Installers
{
    internal static class ArdalisListStartupServicesInstaller
    {
        public static IServiceCollection AddListStartupServicesInstaller(this IServiceCollection services)
        {
            services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(services);

                config.Path = "/allservices";
            });

            return services;
        }

        public static IApplicationBuilder UseListStartupServicesInstaller(this IApplicationBuilder app)
        {
            app.UseShowAllServicesMiddleware();

            return app;
        }
    }
}
