using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using sw.logging.api.Helpers;
using sw.logging.api.Models;
using sw.infrastructure.Exceptions.Repositories.Marten;
using Marten;
using Weasel.Core;
using sw.logging.api.Repositories;

namespace sw.logging.api.Configurations;

internal static class Config
{
    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
          builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        });
        return services;
    }
}