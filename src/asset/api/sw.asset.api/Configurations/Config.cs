using sw.asset.api.Helpers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.asset.contracts.V1.PersonProcessors;
using sw.asset.contracts.V1.SensorProcessors;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.asset.repository.Mappings.Assets;
using sw.asset.repository.NhUnitOfWork;
using sw.asset.repository.Repositories;
using sw.asset.services.V1.AssetCategoryService;
using sw.asset.services.V1.AssetServices.ContainerService;
using sw.asset.services.V1.AssetServices.VehicleService;
using sw.asset.services.V1.CompanyService;
using sw.asset.services.V1.DeviceModelService;
using sw.asset.services.V1.DeviceService;
using sw.asset.services.V1.PersonService;
using sw.asset.services.V1.SensorService;
using sw.asset.services.V1.SensorTypeService;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using sw.infrastructure.Serializers;
using sw.infrastructure.UnitOfWorks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Metadata;
using System;
using System.Reflection;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.asset.services.V1.SimcardService;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.asset.contracts.V1.CompanyProcessors.ZoneProcessors;
using sw.asset.repository.Repositories.Geofence;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.asset.services.V1.GeofenceService;
using sw.asset.contracts.V1.EventProcessors;
using sw.asset.services.V1.CompanyService.ZoneService;
using sw.asset.services.V1.EventService;

namespace sw.asset.api.Configurations;

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
                //.ShowSql()
                .MaxFetchDepth(5)
                .FormatSql()
                .Raw("transaction.use_connection_on_system_prepare", "true")
                .AdoNetBatchSize(100)
              )
              .Mappings(x => x.FluentMappings.AddFromAssemblyOf<AssetMap>())
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
        services.ConfigureCompanyContracts();
        services.ConfigureZoneContracts();
        services.ConfigureContainerContracts();
        services.ConfigureVehicleContracts();
        services.ConfigureAssetCategoryContracts();
        services.ConfigureDeviceModelContracts();
        services.ConfigureDeviceContracts();
        services.ConfigureSimcardContracts();
        services.ConfigureSensorTypeContracts();
        services.ConfigureSensorContracts();
        services.ConfigurePersonContracts();
        services.ConfigureGeofenceContracts();
        services.ConfigureEventHistoryContracts();

        return services;
    }

    private static IServiceCollection ConfigureCompanyContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateCompanyProcessor, CreateCompanyProcessor>();
        services.AddScoped<IUpdateCompanyProcessor, UpdateCompanyProcessor>();
        services.AddScoped<IDeleteSoftCompanyProcessor, DeleteSoftCompanyProcessor>();
        services.AddScoped<IDeleteHardCompanyProcessor, DeleteHardCompanyProcessor>();
        services.AddScoped<IGetCompanyByIdProcessor, GetCompanyByIdProcessor>();
        services.AddScoped<IGetCompaniesProcessor, GetCompaniesProcessor>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        return services;
    }

    private static IServiceCollection ConfigureZoneContracts(this IServiceCollection services)
    {
        services.AddScoped<IGetZonesProcessor, GetZonesProcessor>();
        services.AddScoped<IZoneRepository, ZoneRepository>();
        return services;
    }

    private static IServiceCollection ConfigurePersonContracts(this IServiceCollection services)
    {
        services.AddScoped<IGetPersonByEmailProcessor, GetPersonByEmailProcessor>();
        services.AddScoped<IGetPersonByIdProcessor, GetPersonByIdProcessor>();
        services.AddScoped<IGetPersonsProcessor, GetPersonsProcessor>();

        services.AddScoped<IPersonRepository, PersonRepository>();
        return services;
    }

    private static IServiceCollection ConfigureContainerContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateContainerProcessor, CreateContainerProcessor>();
        services.AddScoped<IUpdateContainerProcessor, UpdateContainerProcessor>();
        services.AddScoped<IDeleteSoftContainerProcessor, DeleteSoftContainerProcessor>();
        services.AddScoped<IDeleteHardContainerProcessor, DeleteHardContainerProcessor>();
        services.AddScoped<IGetContainerByIdProcessor, GetContainerByIdProcessor>();
        services.AddScoped<IGetContainerByImeiProcessor, GetContainerByImeiProcessor>();
        services.AddScoped<IGetContainersCountTotalInZoneProcessor, GetContainersCountTotalInZoneProcessor>();
        services.AddScoped<IGetContainersProcessor, GetContainersProcessor>();
        services.AddScoped<IGetContainersCountTotalProcessor, GetContainersCountTotalProcessor>();
        services.AddScoped<ISearchContainersBetweenLevelsProcessor, SearchContainersBetweenLevelsProcessor>();
        services.AddScoped<ISearchContainersWithCriteriaProcessor, SearchContainersWithCriteriaProcessor>();

        services.AddScoped<IContainerRepository, ContainerRepository>();
        return services;
    }

    private static IServiceCollection ConfigureVehicleContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateVehicleProcessor, CreateVehicleProcessor>();
        services.AddScoped<IUpdateVehicleProcessor, UpdateVehicleProcessor>();
        services.AddScoped<IDeleteSoftVehicleProcessor, DeleteSoftVehicleProcessor>();
        services.AddScoped<IDeleteHardVehicleProcessor, DeleteHardVehicleProcessor>();
        services.AddScoped<IGetVehicleByIdProcessor, GetVehicleByIdProcessor>();
        services.AddScoped<IGetVehiclesProcessor, GetVehiclesProcessor>();

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        return services;
    }

    private static IServiceCollection ConfigureAssetCategoryContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateAssetCategoryProcessor, CreateAssetCategoryProcessor>();
        services.AddScoped<IUpdateAssetCategoryProcessor, UpdateAssetCategoryProcessor>();
        services.AddScoped<IDeleteSoftAssetCategoryProcessor, DeleteSoftAssetCategoryProcessor>();
        services.AddScoped<IDeleteHardAssetCategoryProcessor, DeleteHardAssetCategoryProcessor>();
        services.AddScoped<IGetAssetCategoryByIdProcessor, GetAssetCategoryByIdProcessor>();
        services.AddScoped<IGetAssetCategoriesProcessor, GetAssetCategoriesProcessor>();

        services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();
        return services;
    }

    private static IServiceCollection ConfigureDeviceModelContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateDeviceModelProcessor, CreateDeviceModelProcessor>();
        services.AddScoped<IUpdateDeviceModelProcessor, UpdateDeviceModelProcessor>();
        services.AddScoped<IDeleteSoftDeviceModelProcessor, DeleteSoftDeviceModelProcessor>();
        services.AddScoped<IDeleteHardDeviceModelProcessor, DeleteHardDeviceModelProcessor>();
        services.AddScoped<IGetDeviceModelByIdProcessor, GetDeviceModelByIdProcessor>();
        services.AddScoped<IGetDeviceModelsProcessor, GetDeviceModelsProcessor>();

        services.AddScoped<IDeviceModelRepository, DeviceModelRepository>();
        return services;
    }

    private static IServiceCollection ConfigureDeviceContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateDeviceProcessor, CreateDeviceProcessor>();
        services.AddScoped<IUpdateDeviceProcessor, UpdateDeviceProcessor>();
        services.AddScoped<IDeleteSoftDeviceProcessor, DeleteSoftDeviceProcessor>();
        services.AddScoped<IDeleteHardDeviceProcessor, DeleteHardDeviceProcessor>();
        services.AddScoped<IGetDeviceByIdProcessor, GetDeviceByIdProcessor>();
        services.AddScoped<IGetDevicesProcessor, GetDeviceProcessor>();

        services.AddScoped<IDeviceRepository, DeviceRepository>();
        return services;
    }

    private static IServiceCollection ConfigureSimcardContracts(this IServiceCollection services)
    {
      services.AddScoped<ICreateSimcardProcessor, CreateSimcardProcessor>();
      services.AddScoped<IUpdateSimcardProcessor, UpdateSimcardProcessor>();
      services.AddScoped<IDeleteSoftSimcardProcessor, DeleteSoftSimcardProcessor>();
      services.AddScoped<IDeleteHardSimcardProcessor, DeleteHardSimcardProcessor>();
      services.AddScoped<IGetSimcardByIdProcessor, GetSimcardByIdProcessor>();
      services.AddScoped<IGetSimcardsProcessor, GetSimcardProcessor>();

      services.AddScoped<ISimcardRepository, SimcardRepository>();
      return services;
    }

    private static IServiceCollection ConfigureSensorTypeContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateSensorTypeProcessor, CreateSensorTypeProcessor>();
        services.AddScoped<IUpdateSensorTypeProcessor, UpdateSensorTypeProcessor>();
        services.AddScoped<IDeleteSoftSensorTypeProcessor, DeleteSoftSensorTypeProcessor>();
        services.AddScoped<IDeleteHardSensorTypeProcessor, DeleteHardSensorTypeProcessor>();
        services.AddScoped<IGetSensorTypeByIdProcessor, GetSensorTypeByIdProcessor>();
        services.AddScoped<IGetSensorTypesProcessor, GetSensorTypesProcessor>();


        services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
        return services;
    }

    private static IServiceCollection ConfigureEventHistoryContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateEventHistoryProcessor, CreateEventHistoryProcessor>();
        services.AddScoped<IUpdateEventHistoryProcessor, UpdateEventHistoryProcessor>();
        services.AddScoped<IDeleteHardEventHistoryProcessor, DeleteHardEventHistoryProcessor>();
        services.AddScoped<IGetEventHistoryByIdProcessor, GetEventHistoryByIdProcessor>();
        services.AddScoped<IGetEventHistoryProcessor, GetEventHistoryProcessor>();
        services.AddScoped<IGetEventHistoryByContainerIdProcessor, GetEventHistoryByContainerIdProcessor>();

        services.AddScoped<IEventHistoryRepository, EventHistoryRepository>();
        return services;
    }

    private static IServiceCollection ConfigureSensorContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateSensorProcessor, CreateSensorProcessor>();
        services.AddScoped<IUpdateSensorProcessor, UpdateSensorProcessor>();
        services.AddScoped<IDeleteSoftSensorProcessor, DeleteSoftSensorProcessor>();
        services.AddScoped<IDeleteHardSensorProcessor, DeleteHardSensorProcessor>();
        services.AddScoped<IGetSensorByIdProcessor, GetSensorByIdProcessor>();
        services.AddScoped<IGetSensorsProcessor, GetSensorsProcessor>();

        services.AddScoped<ISensorRepository, SensorRepository>();
        return services;
    }
    private static IServiceCollection ConfigureGeofenceContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateGeofenceProcessor, CreateGeofenceProcessor>();
        services.AddScoped<IUpdateGeofenceProcessor, UpdateGeofenceProcessor>();
        services.AddScoped<IGetGeofencesProcessor, GetGeofencesProcessor>();
        services.AddScoped<IGetGeofenceByKeyProcessor, GetGeofenceByKeyProcessor>();
        services.AddScoped<IGetGeoEntryByKeyProcessor, GetGeoEntryByKeyProcessor>();

        services.AddScoped<IGeofenceRedisRepository, GeofenceRedisRepository>();
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
}//Class: Config
