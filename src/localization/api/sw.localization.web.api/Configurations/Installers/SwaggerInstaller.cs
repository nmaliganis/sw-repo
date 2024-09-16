using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace sw.localization.api.Configurations.Installers {
    /// <summary>
    /// Class : SwaggerInstaller
    /// </summary>
    public static class SwaggerInstaller {
        /// <summary>
        /// Method : AddSwaggerInstaller
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerInstaller(this IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo() {
                    Title = "sw.localization.api - HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API for sw.localization.api service",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Description =
                        "Authorization: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        /// <summary>
        /// Method : UseSwaggerInstaller
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerInstaller(this IApplicationBuilder app) {
            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "sw.localization.api - HTTP API");
            });

            return app;
        }
    }
}
