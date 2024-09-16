using AutoMapper;
using sw.routing.api.Configurations.AutoMappingProfiles.Drivers;
using sw.routing.api.Configurations.AutoMappingProfiles.DriversTransportCombinations;
using sw.routing.api.Configurations.AutoMappingProfiles.Itineraries;
using sw.routing.api.Configurations.AutoMappingProfiles.ItineraryTemplates;
using sw.routing.api.Configurations.AutoMappingProfiles.Jobs;
using sw.routing.api.Configurations.AutoMappingProfiles.Locations;
using sw.routing.api.Configurations.AutoMappingProfiles.TransportCombinations;
using sw.routing.api.Configurations.AutoMappingProfiles.Vehicles;
using sw.routing.model.TransportCombinations;
using sw.infrastructure.TypeMappings;
using Microsoft.Extensions.DependencyInjection;

namespace sw.routing.api.Configurations.Installers;

internal static class AutoMapperInstaller
{
    public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            // Itinerary
            cfg.AddProfile<CreateItineraryCommandToItineraryAutoMapperProfile>();
            cfg.AddProfile<ItineraryToItineraryUiAutoMapperProfile>();
            cfg.AddProfile<ItineraryToItineraryModificationUiAutoMapperProfile>();     
            
            // Itinerary Templates
            cfg.AddProfile<ItineraryTemplateToItineraryTemplateUiAutoMapperProfile>();
            cfg.AddProfile<ItineraryTemplateLocationToItineraryTemplateLocationUiAutoMapperProfile>();

            // Location
            cfg.AddProfile<LocationToLocationUiAutoMapperProfile>();

            // Job
            cfg.AddProfile<JobToJobUiAutoMapperProfile>();

            // TransportCombination
            cfg.AddProfile<TransportCombinationToTransportCombinationUiAutoMapperProfile>();

            // Vehicle
            cfg.AddProfile<VehicleToVehicleUiAutoMapperProfile>();
            cfg.AddProfile<VehicleTransportCombinationToVehicleTransportCombinationUiAutoMapperProfile>();

            // Drivers
            cfg.AddProfile<DriverToDriverUiAutoMapperProfile>();
            cfg.AddProfile<DriverTransportCombinationToDriverTransportCombinationUiAutoMapperProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

        return services;
    }
}