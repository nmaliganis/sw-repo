using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sw.admin.api.Configurations.Installers
{
    internal static class HealthCheckInstaller
    {
        public static IServiceCollection AddNpgSqlHealthCheckInstaller(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("PostgreSqlDatabase"),
                    failureStatus: HealthStatus.Unhealthy,
                    name: "PostgreSQL database", tags: new[] { "ready" });

            return services;
        }

        public static IApplicationBuilder UseHealthCheckInstaller(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health/ready",
                    new HealthCheckOptions
                    {

                        ResultStatusCodes =
                        {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                        },

                        Predicate = (check) => check.Tags.Contains("ready"),
                        AllowCachingResponses = false,
                        ResponseWriter = WriteHealthCheckReadyResponse
                    });

            app.UseHealthChecks("/health/live",
                new HealthCheckOptions
                {
                    Predicate = (check) => !check.Tags.Contains("live"),
                    ResponseWriter = WriteHealthCheckLiveResponse,
                    AllowCachingResponses = false
                });

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                },
                AllowCachingResponses = false
            });

            return app;
        }


        private static Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("OverallStatus", result.Status.ToString()),
                new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00"))
            );

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }

        private static Task WriteHealthCheckReadyResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("OverallStatus", result.Status.ToString()),
                new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00")),
                new JProperty("DependencyHealthChecks", new JObject(result.Entries.Select(dicItem =>
                    new JProperty(dicItem.Key, new JObject(
                        new JProperty("Status", dicItem.Value.Status.ToString()),
                        new JProperty("Duration", dicItem.Value.Duration.TotalSeconds.ToString("0:0.00")),
                        new JProperty("Exception", dicItem.Value.Exception?.Message),
                        new JProperty("Data", new JObject(dicItem.Value.Data.Select(dicData =>
                            new JProperty(dicData.Key, dicData.Value))))
                    ))
                )))
            );

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}
