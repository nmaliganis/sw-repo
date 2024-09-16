using System;
using sw.localization.api.Helpers;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationDomainProcessors;
using sw.localization.contracts.V1.LocalizationLanguageProcessors;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.localization.repository;
using sw.localization.repository.EfUnitOfWork;
using sw.localization.repository.Repositories;
using sw.localization.services.LocalizationDomainService;
using sw.localization.services.LocalizationLanguageService;
using sw.localization.services.LocalizationValueService;
using sw.infrastructure.Exceptions.Repositories.Marten;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using sw.infrastructure.Serializers;
using sw.infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace sw.localization.api.Configurations
{
    /// <summary>
    /// Class : Config
    /// </summary>
    public static class Config
    {
        public static IServiceCollection ConfigureEfCoreWithNpgsql(this IServiceCollection services, string connectionString)
        {
            try
            {
                services.AddDbContext<swDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(connectionString, opt =>
                    {
                        opt.MaxBatchSize(100)
                           .SetPostgresVersion(new Version("11.11"));
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

        /// <summary>
        /// Method : ConfigureInfrastructure
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
            services.AddSingleton<ITypeHelperService, TypeHelperService>();
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }

        /// <summary>
        /// Method : ConfigureContracts
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureContracts(this IServiceCollection services)
        {
            services.ConfigureLocalizationValueContracts();
            services.ConfigureLocalizationLanguageContracts();
            services.ConfigureLocalizationDomainContracts();

            return services;
        }

        private static IServiceCollection ConfigureLocalizationValueContracts(this IServiceCollection services)
        {
            services.AddScoped<IGetLocalizationValuesProcessor, GetLocalizationValuesProcessor>();
            services.AddScoped<IGetLocalizationValueByIdProcessor, GetLocalizationValueByIdProcessor>();
            services.AddScoped<IGetLocalizationValueByKeyProcessor, GetLocalizationValueByKeyProcessor>();

            services.AddScoped<ICreateLocalizationValueProcessor, CreateLocalizationValueProcessor>();
            services.AddScoped<IUpdateLocalizationValueProcessor, UpdateLocalizationValueProcessor>();
            services.AddScoped<IDeleteLocalizationvalueProcessor, DeleteLocalizationvalueProcessor>();
            services.AddScoped<IHardDeleteLocalizationValueProcessor, HardDeleteLocalizationvalueProcessor>();

            services.AddScoped<ILocalizationValueRepository, LocalizationValueRepository>();

            return services;
        }

        private static IServiceCollection ConfigureLocalizationLanguageContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateLocalizationLanguageProcessor, CreateLocalizationLanguageProcessor>();
            services.AddScoped<IHardDeleteLocalizationLanguageProcessor, HardDeleteLocalizationLanguageProcessor>();

            services.AddScoped<ILocalizationLanguageRepository, LocalizationLanguageRepository>();

            return services;
        }

        private static IServiceCollection ConfigureLocalizationDomainContracts(this IServiceCollection services)
        {
            services.AddScoped<ICreateLocalizationDomainProcessor, CreateLocalizationDomainProcessor>();
            services.AddScoped<IHardDeleteLocalizationDomainProcessor, HardDeleteLocalizationDomainProcessor>();

            services.AddScoped<ILocalizationDomainRepository, LocalizationDomainRepository>();

            return services;
        }
    }
}