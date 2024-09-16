using System;
using System.Collections.Generic;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.model.Itineraries;
using sw.routing.model.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.PropertyMappings;

namespace sw.routing.api.Helpers;

/// <summary>
/// Class : PropertyMappingService
/// </summary>
public class PropertyMappingService : BasePropertyMapping
{
    private readonly Dictionary<string, PropertyMappingValue> _itineraryPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
        };

    private readonly Dictionary<string, PropertyMappingValue> _itineraryTemplatePropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
        };

    private readonly Dictionary<string, PropertyMappingValue> _locationPropertyMapping =
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
        PropertyMappings.Add(new PropertyMapping<ItineraryUiModel, Itinerary>(_itineraryPropertyMapping));
        PropertyMappings.Add(new PropertyMapping<ItineraryTemplateUiModel, ItineraryTemplate>(_itineraryTemplatePropertyMapping));
        PropertyMappings.Add(new PropertyMapping<LocationUiModel, LocationPoint>(_locationPropertyMapping));
    }
}