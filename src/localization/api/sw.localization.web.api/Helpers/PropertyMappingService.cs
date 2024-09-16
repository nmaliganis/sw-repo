using System;
using System.Collections.Generic;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.model.Localizations;
using sw.infrastructure.PropertyMappings;

namespace sw.localization.api.Helpers
{
    /// <summary>
    /// Class : PropertyMappingService
    /// </summary>
    public class PropertyMappingService : BasePropertyMapping
    {
        private readonly Dictionary<string, PropertyMappingValue> _localizationValuePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Key", new PropertyMappingValue(new List<string>() {"Key"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _localizationLanguagePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                        {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
            };

        private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        /// <summary>
        /// PropertyMappingService
        /// </summary>
        public PropertyMappingService() : base(PropertyMappings)
        {
            PropertyMappings.Add(new PropertyMapping<LocalizationValueUiModel, LocalizationValue>(_localizationValuePropertyMapping));
            PropertyMappings.Add(new PropertyMapping<LocalizationLanguageUiModel, LocalizationLanguage>(_localizationLanguagePropertyMapping));
        }
    }//Class : PropertyMappingService
}//Namespace : sw.localization.api.Helpers
