using AutoMapper;
using sw.landmark.web.api.Configurations.AutoMappingProfiles.EventHistories;
using sw.landmark.web.api.Configurations.AutoMappingProfiles.EventPositions;
using sw.landmark.web.api.Configurations.AutoMappingProfiles.GeocodedPositions;
using sw.landmark.web.api.Configurations.AutoMappingProfiles.GeocoderProfiles;
using sw.landmark.web.api.Configurations.AutoMappingProfiles.LandmarkCategories;
using sw.landmark.web.api.Configurations.AutoMappingProfiles.Landmarks;
using sw.infrastructure.TypeMappings;
using Microsoft.Extensions.DependencyInjection;

namespace sw.landmark.web.api.Configurations.Installers
{
    internal static class AutoMapperInstaller
    {
        public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // EventHistory
                cfg.AddProfile<EventHistoryToEventHistoryUiAutoMapperProfile>();
                cfg.AddProfile<EventHistoryToEventHistoryCreationUiModelAutoMapperProfile>();
                cfg.AddProfile<EventHistoryToEventHistoryModificationUiAutoMapperProfile>();
                cfg.AddProfile<CreateEventHistoryCommandToEventHistoryAutoMapperProfile>();
                cfg.AddProfile<UpdateEventHistoryCommandToEventHistoryAutoMapperProfile>();

                // EventPosition
                cfg.AddProfile<EventPositionToEventPositionUiAutoMapperProfile>();
                cfg.AddProfile<EventPositionToEventPositionCreationUiModelAutoMapperProfile>();
                cfg.AddProfile<EventPositionToEventPositionModificationUiAutoMapperProfile>();
                cfg.AddProfile<CreateEventPositionCommandToEventPositionAutoMapperProfile>();
                cfg.AddProfile<UpdateEventPositionCommandToEventPositionAutoMapperProfile>();

                // GeocodedPosition
                cfg.AddProfile<GeocodedPositionToGeocodedPositionUiAutoMapperProfile>();
                cfg.AddProfile<GeocodedPositionToGeocodedPositionCreationUiModelAutoMapperProfile>();
                cfg.AddProfile<GeocodedPositionToGeocodedPositionModificationUiAutoMapperProfile>();
                cfg.AddProfile<CreateGeocodedPositionCommandToGeocodedPositionAutoMapperProfile>();
                cfg.AddProfile<UpdateGeocodedPositionCommandToGeocodedPositionAutoMapperProfile>();

                // GeocoderProfile
                cfg.AddProfile<GeocoderProfileToGeocoderProfileUiAutoMapperProfile>();
                cfg.AddProfile<GeocoderProfileToGeocoderProfileCreationUiModelAutoMapperProfile>();
                cfg.AddProfile<GeocoderProfileToGeocoderProfileModificationUiAutoMapperProfile>();
                cfg.AddProfile<CreateGeocoderProfileCommandToGeocoderProfileAutoMapperProfile>();
                cfg.AddProfile<UpdateGeocoderProfileCommandToGeocoderProfileAutoMapperProfile>();

                // Landmark
                cfg.AddProfile<LandmarkToLandmarkCreationUiModelAutoMapperProfile>();
                cfg.AddProfile<LandmarkToLandmarkModificationUiAutoMapperProfile>();
                cfg.AddProfile<CreateLandmarkCommandToLandmarkAutoMapperProfile>();
                cfg.AddProfile<UpdateLandmarkCommandToLandmarkAutoMapperProfile>();
                cfg.AddProfile<LandmarkToLandmarkUiAutoMapperProfile>();

                // LandmarkCategory
                cfg.AddProfile<LandmarkCategoryToLandmarkCategoryUiAutoMapperProfile>();
                cfg.AddProfile<LandmarkCategoryToLandmarkCategoryCreationUiModelAutoMapperProfile>();
                cfg.AddProfile<LandmarkCategoryToLandmarkCategoryModificationUiAutoMapperProfile>();
                cfg.AddProfile<CreateLandmarkCategoryCommandToLandmarkCategoryAutoMapperProfile>();
                cfg.AddProfile<UpdateLandmarkCategoryCommandToLandmarkCategoryAutoMapperProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

            return services;
        }
    }
}
