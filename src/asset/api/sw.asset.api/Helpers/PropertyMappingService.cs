using System;
using System.Collections.Generic;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.model.Assets.Categories;
using sw.asset.model.Assets.Containers;
using sw.asset.model.Assets.Vehicles;
using sw.asset.model.Companies;
using sw.asset.model.Devices;
using sw.asset.model.Devices.Simcards;
using sw.asset.model.Events;
using sw.asset.model.Sensors;
using sw.asset.model.SensorTypes;
using sw.infrastructure.PropertyMappings;

namespace sw.asset.api.Helpers;

/// <summary>
/// Class : PropertyMappingService
/// </summary>
public class PropertyMappingService : BasePropertyMapping
{
  private readonly Dictionary<string, PropertyMappingValue> _companyPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _containerPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
      {"Status", new PropertyMappingValue(new List<string>() {"Status"})},
      {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
      {"Level", new PropertyMappingValue(new List<string>() {"Level"})},
      {"Capacity", new PropertyMappingValue(new List<string>() {"Capacity"})},
      {"LastServicedDate", new PropertyMappingValue(new List<string>() {"LastServicedDate"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _vehiclePropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _assetCategoryPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _deviceModelPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _devicePropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _simcardPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _sensorTypePropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

  private readonly Dictionary<string, PropertyMappingValue> _sensorPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
    };

    private readonly Dictionary<string, PropertyMappingValue> _eventHistoryPropertyMapping =
      new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
      {
          {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
      };


    private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

  /// <summary>
  /// PropertyMappingService
  /// </summary>
  public PropertyMappingService() : base(PropertyMappings)
  {
    PropertyMappings.Add(new PropertyMapping<CompanyUiModel, Company>(_companyPropertyMapping));
    PropertyMappings.Add(new PropertyMapping<ContainerUiModel, Container>(_containerPropertyMapping));
    PropertyMappings.Add(new PropertyMapping<VehicleUiModel, Vehicle>(_vehiclePropertyMapping));
    PropertyMappings.Add(new PropertyMapping<AssetCategoryUiModel, AssetCategory>(_assetCategoryPropertyMapping));
    PropertyMappings.Add(new PropertyMapping<DeviceModelUiModel, DeviceModel>(_deviceModelPropertyMapping));
    PropertyMappings.Add(new PropertyMapping<SimcardUiModel, Simcard>(_simcardPropertyMapping));
    PropertyMappings.Add(new PropertyMapping<DeviceUiModel, Device>(_devicePropertyMapping));
    PropertyMappings.Add(new PropertyMapping<SensorTypeUiModel, SensorType>(_sensorTypePropertyMapping));
    PropertyMappings.Add(new PropertyMapping<SensorUiModel, Sensor>(_sensorPropertyMapping));
    PropertyMappings.Add(new PropertyMapping<EventHistoryUiModel, EventHistory>(_eventHistoryPropertyMapping));
    }
}