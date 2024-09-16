using System;
using sw.landmark.api.Helpers;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventHistoryProcessors;
using sw.landmark.contracts.V1.EventPositionProcessors;
using sw.landmark.contracts.V1.GeocodedPositionProcessors;
using sw.landmark.contracts.V1.GeocoderProfileProcessors;
using sw.landmark.contracts.V1.LandmarkCategoryProcessors;
using sw.landmark.contracts.V1.LandmarkProcessors;
using sw.landmark.repository.DbContexts;
using sw.landmark.repository.EfUnitOfWork;
using sw.landmark.repository.Repositories;
using sw.landmark.services.V1.EventHistoryService;
using sw.landmark.services.V1.EventPositionService;
using sw.landmark.services.V1.GeocodedPositionService;
using sw.landmark.services.V1.GeocoderProfileService;
using sw.landmark.services.V1.LandmarkCategoryService;
using sw.landmark.services.V1.LandmarkService;
using sw.infrastructure.Exceptions.Repositories.Marten;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using sw.infrastructure.Serializers;
using sw.infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace sw.landmark.api.Configurations
{
    internal static class Config
    {
        internal static IServiceCollection ConfigureEfCoreWithNpgsql(this IServiceCollection services, string connectionString)
        {
            try
            {
                services.AddDbContext<swDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(connectionString, opt =>
                    {
                        opt.MaxBatchSize(100)
                           .SetPostgresVersion(new Version("11.11"))
                           .UseNetTopologySuite();
                    })
                    .UseLowerCaseNamingConvention()
                );

                return services;
            }
            catch (Exception ex)
            {
                throw new NHibernateInitializationException(ex.Message, ex.InnerException?.Message);
            }
        }

        internal static IServiceCollection CongifureInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
            services.AddSingleton<ITypeHelperService, TypeHelperService>();
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }

        internal static IServiceCollection ConfigureContracts(this IServiceCollection services)
        {
            services.ConfigureEventHistoryContracts();
            services.ConfigureEventPositionContracts();
            services.ConfigureGeocoderProfileContracts();
            services.ConfigureLandmarkContracts();
            services.ConfigureLandmarkCategoryContracts();
            services.ConfigureGeocodedPositionContracts();

            return services;
        }

        private static IServiceCollection ConfigureEventHistoryContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateEventHistoryProcessor, CreateEventHistoryProcessor>();
            services.AddScoped<IUpdateEventHistoryProcessor, UpdateEventHistoryProcessor>();
            services.AddScoped<IDeleteSoftEventHistoryProcessor, DeleteSoftEventHistoryProcessor>();
            services.AddScoped<IDeleteHardEventHistoryProcessor, DeleteHardEventHistoryProcessor>();
            services.AddScoped<IGetEventHistoryByIdProcessor, GetEventHistoryByIdProcessor>();
            services.AddScoped<IGetEventHistoriesProcessor, GetEventHistoriesProcessor>();

            services.AddScoped<IEventHistoryRepository, EventHistoryRepository>();
            return services;
        }

        private static IServiceCollection ConfigureEventPositionContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateEventPositionProcessor, CreateEventPositionProcessor>();
            services.AddScoped<IUpdateEventPositionProcessor, UpdateEventPositionProcessor>();
            services.AddScoped<IDeleteSoftEventPositionProcessor, DeleteSoftEventPositionProcessor>();
            services.AddScoped<IDeleteHardEventPositionProcessor, DeleteHardEventPositionProcessor>();
            services.AddScoped<IGetEventPositionByIdProcessor, GetEventPositionByIdProcessor>();
            services.AddScoped<IGetEventPositionsProcessor, GetEventPositionsProcessor>();

            services.AddScoped<IEventPositionRepository, EventPositionRepository>();
            return services;
        }

        private static IServiceCollection ConfigureGeocodedPositionContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateGeocodedPositionProcessor, CreateGeocodedPositionProcessor>();
            services.AddScoped<IUpdateGeocodedPositionProcessor, UpdateGeocodedPositionProcessor>();
            services.AddScoped<IDeleteSoftGeocodedPositionProcessor, DeleteSoftGeocodedPositionProcessor>();
            services.AddScoped<IDeleteHardGeocodedPositionProcessor, DeleteHardGeocodedPositionProcessor>();
            services.AddScoped<IGetGeocodedPositionByIdProcessor, GetGeocodedPositionByIdProcessor>();
            services.AddScoped<IGetGeocodedPositionsProcessor, GetGeocodedPositionsProcessor>();

            services.AddScoped<IGeocodedPositionRepository, GeocodedPositionRepository>();
            return services;
        }

        private static IServiceCollection ConfigureGeocoderProfileContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateGeocoderProfileProcessor, CreateGeocoderProfileProcessor>();
            services.AddScoped<IUpdateGeocoderProfileProcessor, UpdateGeocoderProfileProcessor>();
            services.AddScoped<IDeleteSoftGeocoderProfileProcessor, DeleteSoftGeocoderProfileProcessor>();
            services.AddScoped<IDeleteHardGeocoderProfileProcessor, DeleteHardGeocoderProfileProcessor>();
            services.AddScoped<IGetGeocoderProfileByIdProcessor, GetGeocoderProfileByIdProcessor>();
            services.AddScoped<IGetGeocoderProfilesProcessor, GetGeocoderProfilesProcessor>();

            services.AddScoped<IGeocoderProfileRepository, GeocoderProfileRepository>();
            return services;
        }

        private static IServiceCollection ConfigureLandmarkContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateLandmarkProcessor, CreateLandmarkProcessor>();
            services.AddScoped<IUpdateLandmarkProcessor, UpdateLandmarkProcessor>();
            services.AddScoped<IDeleteSoftLandmarkProcessor, DeleteSoftLandmarkProcessor>();
            services.AddScoped<IDeleteHardLandmarkProcessor, DeleteHardLandmarkProcessor>();
            services.AddScoped<IGetLandmarkByIdProcessor, GetLandmarkByIdProcessor>();
            services.AddScoped<IGetLandmarksProcessor, GetLandmarksProcessor>();

            services.AddScoped<ILandmarkRepository, LandmarkRepository>();
            return services;
        }

        private static IServiceCollection ConfigureLandmarkCategoryContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateLandmarkCategoryProcessor, CreateLandmarkCategoryProcessor>();
            services.AddScoped<IUpdateLandmarkCategoryProcessor, UpdateLandmarkCategoryProcessor>();
            services.AddScoped<IDeleteSoftLandmarkCategoryProcessor, DeleteSoftLandmarkCategoryProcessor>();
            services.AddScoped<IDeleteHardLandmarkCategoryProcessor, DeleteHardLandmarkCategoryProcessor>();
            services.AddScoped<IGetLandmarkCategoryByIdProcessor, GetLandmarkCategoryByIdProcessor>();
            services.AddScoped<IGetLandmarkCategoriesProcessor, GetLandmarkCategoriesProcessor>();

            services.AddScoped<ILandmarkCategoryRepository, LandmarkCategoryRepository>();
            return services;
        }
    }
}