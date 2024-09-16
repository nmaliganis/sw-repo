using AutoMapper;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.model.Localizations;

namespace sw.localization.api.Configurations.AutoMappingProfiles.LocalizationValues
{
    /// <summary>
    /// Class : LocalizationValueToLocalizationValueCreationUiAutoMapperProfile
    /// </summary>
    public class LocalizationValueToLocalizationValueCreationUiAutoMapperProfile : Profile
    {
        /// <summary>
        /// Ctor : LocalizationValueToLocalizationValueCreationUiAutoMapperProfile
        /// </summary>
        public LocalizationValueToLocalizationValueCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        /// <summary>
        /// Method : ConfigureMapping
        /// </summary>
        public void ConfigureMapping()
        {
            CreateMap<LocalizationValue, LocalizationValueCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }//Class : LocalizationValueToLocalizationValueCreationUiAutoMapperProfile

}//Namespace : sw.localization.api.Configurations.AutoMappingProfiles.LocalizationValues