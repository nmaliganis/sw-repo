using System.Collections.Generic;
using Ardalis.ListStartupServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace sw.localization.api.Configurations.Installers
{
    /// <summary>
    /// Class : ArdalisListStartupServicesInstaller
    /// </summary>
    public static class ArdalisListStartupServicesInstaller
    {
        /// <summary>
        /// Method : AddListStartupServicesInstaller
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddListStartupServicesInstaller(this IServiceCollection services)
        {
            services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(services);

                config.Path = "/allservices";
            });

            return services;
        }

        /// <summary>
        /// Method : UseListStartupServicesInstaller
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseListStartupServicesInstaller(this IApplicationBuilder app)
        {
            app.UseShowAllServicesMiddleware();

            return app;
        }
    }//Class : ArdalisListStartupServicesInstaller
}//Namespace : sw.localization.api.Configurations.Installers
