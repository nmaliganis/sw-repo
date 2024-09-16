using AutoMapper;
using sw.asset.api.Configurations.AutoMappingProfiles.AssetCategories;
using sw.asset.api.Configurations.AutoMappingProfiles.Assets.Containers;
using sw.asset.api.Configurations.AutoMappingProfiles.Assets.Vehicles;
using sw.asset.api.Configurations.AutoMappingProfiles.Companies;
using sw.asset.api.Configurations.AutoMappingProfiles.Companies.Zones;
using sw.asset.api.Configurations.AutoMappingProfiles.DeviceModels;
using sw.asset.api.Configurations.AutoMappingProfiles.Devices;
using sw.asset.api.Configurations.AutoMappingProfiles.Events;
using sw.asset.api.Configurations.AutoMappingProfiles.Geofences;
using sw.asset.api.Configurations.AutoMappingProfiles.Persons;
using sw.asset.api.Configurations.AutoMappingProfiles.Sensors;
using sw.asset.api.Configurations.AutoMappingProfiles.SensorTypes;
using sw.asset.api.Configurations.AutoMappingProfiles.Simcards;
using sw.infrastructure.TypeMappings;
using Microsoft.Extensions.DependencyInjection;

namespace sw.asset.api.Configurations.Installers;

internal static class AutoMapperInstaller
{
    public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            // Company
            cfg.AddProfile<CompanyToCompanyCreationUiAutoMapperProfile>();
            cfg.AddProfile<CompanyToCompanyModificationUiAutoMapperProfile>();
            cfg.AddProfile<CompanyToCompanyUiAutoMapperProfile>();

            // Zone
            cfg.AddProfile<ZoneToZoneUiAutoMapperProfile>();

            // Container
            cfg.AddProfile<ContainerToContainerCreationUiModelAutoMapperProfile>();
            cfg.AddProfile<ContainerToContainerModificationUiAutoMapperProfile>();
            cfg.AddProfile<UpdateContainerCommandToContainerAutoMapperProfile>();
            cfg.AddProfile<ContainerToContainerUiAutoMapperProfile>();

            // Vehicle
            cfg.AddProfile<VehicleToVehicleCreationUiModelAutoMapperProfile>();
            cfg.AddProfile<VehicleToVehicleModificationUiAutoMapperProfile>();
            cfg.AddProfile<CreateVehicleCommandToVehicleAutoMapperProfile>();
            cfg.AddProfile<UpdateVehicleCommandToVehicleAutoMapperProfile>();
            cfg.AddProfile<VehicleToVehicleUiAutoMapperProfile>();

            // AssetCategory
            cfg.AddProfile<AssetCategoryToAssetCategoryCreationUiAutoMapperProfile>();
            cfg.AddProfile<AssetCategoryToAssetCategoryModificationUiAutoMapperProfile>();
            cfg.AddProfile<AssetCategoryToAssetCategoryUiAutoMapperProfile>();

            // DeviceModel
            cfg.AddProfile<DeviceModelToDeviceModelCreationUiAutoMapperProfile>();
            cfg.AddProfile<DeviceModelToDeviceModelModificationUiAutoMapperProfile>();
            cfg.AddProfile<DeviceModelToDeviceModelUiAutoMapperProfile>();

            // Device
            cfg.AddProfile<DeviceToDeviceCreationUiModelAutoMapperProfile>();
            cfg.AddProfile<DeviceToDeviceModificationUiAutoMapperProfile>();
            cfg.AddProfile<CreateDeviceCommandToDeviceAutoMapperProfile>();
            cfg.AddProfile<UpdateDeviceCommandToDeviceAutoMapperProfile>();
            cfg.AddProfile<DeviceToDeviceUiAutoMapperProfile>();

            // Simcard
            cfg.AddProfile<SimcardToSimcardCreationUiModelAutoMapperProfile>();
            cfg.AddProfile<SimcardToSimcardModificationUiAutoMapperProfile>();
            cfg.AddProfile<CreateSimcardCommandToSimcardAutoMapperProfile>();
            cfg.AddProfile<UpdateSimcardCommandToSimcardAutoMapperProfile>();
            cfg.AddProfile<SimcardToSimcardUiAutoMapperProfile>();

            // SensorType
            cfg.AddProfile<SensorTypeToSensorTypeCreationUiModelAutoMapperProfile>();
            cfg.AddProfile<SensorTypeToSensorTypeModificationUiAutoMapperProfile>();
            cfg.AddProfile<CreateSensorTypeCommandToSensorTypeAutoMapperProfile>();
            cfg.AddProfile<UpdateSensorTypeCommandToSensorTypeAutoMapperProfile>();
            cfg.AddProfile<SensorTypeToSensorTypeUiAutoMapperProfile>();

            // EventHistory
            cfg.AddProfile<EventHistoryToEventHistoryCreationUiModelAutoMapperProfile>();
            cfg.AddProfile<UpdateEventHistoryCommandToEventHistoryAutoMapperProfile>();
            cfg.AddProfile<EventHistoryToEventHistoryUiAutoMapperProfile>();
            cfg.AddProfile<EventHistoryToEventHistoryExtraUiAutoMapperProfile>();
            cfg.AddProfile<CreateEventHistoryCommandToEventHistoryAutoMapperProfile>();

            // Sensor
            cfg.AddProfile<SensorToSensorCreationUiModelAutoMapperProfile>();
            cfg.AddProfile<SensorToSensorModificationUiAutoMapperProfile>();
            cfg.AddProfile<CreateSensorCommandToSensorAutoMapperProfile>();
            cfg.AddProfile<UpdateSensorCommandToSensorAutoMapperProfile>();
            cfg.AddProfile<SensorToSensorUiAutoMapperProfile>();

            // Person
            cfg.AddProfile<PersonToPersonUiAutoMapperProfile>();

            //Geofence
            cfg.AddProfile<GeoEntryToGeoEntryUiAutoMapperProfile>();
            cfg.AddProfile<GeofenceToGeofenceUiAutoMapperProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

        return services;
    }
}