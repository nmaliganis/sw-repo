using AutoMapper;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.model.Localizations;

namespace sw.localization.api.Configurations.AutoMappingProfiles.LocalizationValues
{
    /// <summary>
    /// Class
    /// </summary>
    public class LocalizationValueToLocalizationValueUiAutoMapperProfile : Profile
    {
        public LocalizationValueToLocalizationValueUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<LocalizationValue, LocalizationValueUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => src.LocalizationDomain.Name))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.LocalizationLanguage.Name))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}