using sw.routing.api.Helpers;
using sw.routing.repository.NhUnitOfWork;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using sw.infrastructure.Serializers;
using sw.infrastructure.UnitOfWorks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Metadata;
using System;
using System.Reflection;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Itineraries;
using sw.routing.contracts.V1.ItineraryTemplates;
using sw.routing.contracts.V1.Locations;
using sw.routing.repository.Mappings.Jobs;
using sw.routing.repository.Repositories;
using sw.routing.services.V1.Itineraries;
using sw.routing.services.V1.ItineraryTemplates;
using sw.routing.services.V1.Locations;
using ISession = NHibernate.ISession;

namespace sw.routing.api.Configurations;

internal static class Config
{
    public static void ConfigureNHibernateWithNpgsql(this IServiceCollection services, string connectionString)
    {
        HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
        try
        {
            var cfg = Fluently.Configure()
              .Database(PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(connectionString)
                .Driver<NpgsqlDriver>()
                .Dialect<PostGis20Dialect>()
                .ShowSql()
                .MaxFetchDepth(5)
                .FormatSql()
                .Raw("transaction.use_connection_on_system_prepare", "true")
                .AdoNetBatchSize(100)
              )
              .Mappings(x => x.FluentMappings.AddFromAssemblyOf<JobMap>())
              .Cache(c => c.UseSecondLevelCache().UseQueryCache()
                .ProviderClass(typeof(NHibernate.Caches.RtMemoryCache.RtMemoryCacheProvider)
                  .AssemblyQualifiedName)
              )
              .CurrentSessionContext("web")
              .BuildConfiguration();

            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));
            Metadata.AddMapping(cfg, MetadataClass.GeometryColumn);
            Metadata.AddMapping(cfg, MetadataClass.SpatialReferenceSystem);

            var sessionFactory = cfg.BuildSessionFactory();

            services.AddSingleton<ISessionFactory>(sessionFactory);

            services.AddScoped<ISession>((ctx) =>
            {
                var sf = ctx.GetRequiredService<ISessionFactory>();

                return sf.OpenSession();
            });
        }
        catch (Exception ex)
        {
            throw new NHibernateInitializationException(ex.Message, ex.InnerException?.Message);
        }
    }

    internal static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
        services.AddSingleton<ITypeHelperService, TypeHelperService>();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddScoped<IUnitOfWork, NhUnitOfWork>();

        return services;
    }

    internal static IServiceCollection ConfigureContracts(this IServiceCollection services)
    {
        services.ConfigureItineraryContracts();
        services.ConfigureItineraryTemplateContracts();
        services.ConfigureLocationContracts();
        services.ConfigureDriverContracts();
        services.ConfigureVehicleContracts();

        return services;
    }

    private static IServiceCollection ConfigureItineraryContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateItineraryProcessor, CreateItineraryProcessor>();
        services.AddScoped<IUpdateItineraryProcessor, UpdateItineraryProcessor>();
        services.AddScoped<IDeleteSoftItineraryProcessor, DeleteSoftItineraryProcessor>();
        services.AddScoped<IDeleteHardItineraryProcessor, DeleteHardItineraryProcessor>();
        services.AddScoped<IGetItineraryByIdProcessor, GetItineraryByIdProcessor>();
        services.AddScoped<IGetItinerariesProcessor, GetItinerariesProcessor>();

        services.AddScoped<IItineraryRepository, ItineraryRepository>();
        return services;
    }

    private static IServiceCollection ConfigureItineraryTemplateContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateItineraryTemplateProcessor, CreateItineraryTemplateProcessor>();
        services.AddScoped<IUpdateItineraryTemplateProcessor, UpdateItineraryTemplateProcessor>();
        services.AddScoped<IDeleteSoftItineraryTemplateProcessor, DeleteSoftItineraryTemplateProcessor>();
        services.AddScoped<IDeleteHardItineraryTemplateProcessor, DeleteHardItineraryTemplateProcessor>();
        services.AddScoped<IGetItineraryTemplateByIdProcessor, GetItineraryTemplateByIdProcessor>();
        services.AddScoped<IGetItinerariesProcessor, GetItinerariesProcessor>();

        services.AddScoped<IItineraryTemplateRepository, ItineraryTemplateRepository>();
        return services;
    }

    private static IServiceCollection ConfigureLocationContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateLocationProcessor, CreateLocationProcessor>();
        services.AddScoped<IUpdateLocationProcessor, UpdateLocationProcessor>();
        services.AddScoped<IDeleteSoftLocationProcessor, DeleteSoftLocationProcessor>();
        services.AddScoped<IDeleteHardLocationProcessor, DeleteHardLocationProcessor>();
        services.AddScoped<IGetLocationByIdProcessor, GetLocationByIdProcessor>();
        services.AddScoped<IGetItinerariesProcessor, GetItinerariesProcessor>();

        services.AddScoped<ILocationRepository, LocationRepository>();
        return services;
    }
    private static IServiceCollection ConfigureDriverContracts(this IServiceCollection services)
    {
        services.AddScoped<IGetItinerariesProcessor, GetItinerariesProcessor>();

        services.AddScoped<IDriverRepository, DriverRepository>();
        return services;
    }

    private static IServiceCollection ConfigureVehicleContracts(this IServiceCollection services)
    {
        services.AddScoped<IGetItinerariesProcessor, GetItinerariesProcessor>();

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(configuration);

        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers(options =>
          {
              options.Filters.Add(typeof(HttpGlobalExceptionFilter));
          })
          // Added for functional tests
          .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

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