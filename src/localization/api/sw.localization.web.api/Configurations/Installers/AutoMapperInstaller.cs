using AutoMapper;
using sw.infrastructure.TypeMappings;
using Microsoft.Extensions.DependencyInjection;
using LocalizationDomainToLocalizationDomainCreationUiAutoMapperProfile = sw.localization.api.Configurations.AutoMappingProfiles.LocalizationDomains.LocalizationDomainToLocalizationDomainCreationUiAutoMapperProfile;
using LocalizationLanguageToLocalizationLanguageUiAutoMapperProfile = sw.localization.api.Configurations.AutoMappingProfiles.LocalizationLanguages.LocalizationLanguageToLocalizationLanguageCreationUiAutoMapperProfile;
using LocalizationValueToLocalizationValueCreationUiAutoMapperProfile = sw.localization.api.Configurations.AutoMappingProfiles.LocalizationValues.LocalizationValueToLocalizationValueCreationUiAutoMapperProfile;
using LocalizationValueToLocalizationValueModificationUiAutoMapperProfile = sw.localization.api.Configurations.AutoMappingProfiles.LocalizationValues.LocalizationValueToLocalizationValueModificationUiAutoMapperProfile;
using LocalizationValueToLocalizationValueUiAutoMapperProfile = sw.localization.api.Configurations.AutoMappingProfiles.LocalizationValues.LocalizationValueToLocalizationValueUiAutoMapperProfile;

namespace sw.localization.api.Configurations.Installers {
    /// <summary>
    /// Class : AutoMapperInstaller
    /// </summary>
    public static class AutoMapperInstaller {
        public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services) {
            var mapperConfig = new MapperConfiguration(cfg => {
                // Localization Values
                cfg.AddProfile<LocalizationValueToLocalizationValueUiAutoMapperProfile>();
                cfg.AddProfile<LocalizationValueToLocalizationValueCreationUiAutoMapperProfile>();
                cfg.AddProfile<LocalizationValueToLocalizationValueModificationUiAutoMapperProfile>();

                // Localization Languages
                cfg.AddProfile<LocalizationLanguageToLocalizationLanguageUiAutoMapperProfile>();

                // Localization Domains
                cfg.AddProfile<LocalizationDomainToLocalizationDomainCreationUiAutoMapperProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

            return services;
        }
    }
}
