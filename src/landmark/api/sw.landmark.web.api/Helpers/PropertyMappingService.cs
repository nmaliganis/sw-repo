using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.landmark.model;
using sw.infrastructure.PropertyMappings;
using System;
using System.Collections.Generic;

namespace sw.landmark.api.Helpers {
    /// <summary>
    /// Class : PropertyMappingService
    /// </summary>
    public class PropertyMappingService : BasePropertyMapping {
        private readonly Dictionary<string, PropertyMappingValue> _eventHistoryPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                {"Recorded", new PropertyMappingValue(new List<string>() {"Recorded"})},
                {"Received", new PropertyMappingValue(new List<string>() {"Received"})},
                {"Eventvalue", new PropertyMappingValue(new List<string>() {"Eventvalue"})},
                {"EventvalueJson", new PropertyMappingValue(new List<string>() {"EventvalueJson"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _eventPositionPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _geocodedPositionPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _geocoderProfilePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _landmarkPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _landmarkCategoryPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
            };

        private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        /// <summary>
        /// PropertyMappingService
        /// </summary>
        public PropertyMappingService() : base(PropertyMappings) {
            PropertyMappings.Add(new PropertyMapping<EventHistoryUiModel, EventHistory>(this._eventHistoryPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<EventPositionUiModel, EventPosition>(this._eventPositionPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<GeocodedPositionUiModel, GeocodedPosition>(this._geocodedPositionPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<GeocoderProfileUiModel, GeocoderProfile>(this._geocoderProfilePropertyMapping));
            PropertyMappings.Add(new PropertyMapping<LandmarkUiModel, Landmark>(this._landmarkPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<LandmarkCategoryUiModel, LandmarkCategory>(this._landmarkCategoryPropertyMapping));
        }
    }
}
