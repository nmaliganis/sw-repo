using AutoMapper;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.model.Localizations;

namespace sw.localization.api.Configurations.AutoMappingProfiles.LocalizationValues
{
    /// <summary>
    /// Class
    /// </summary>
    public class LocalizationValueToLocalizationValueModificationUiAutoMapperProfile : Profile
    {
        public LocalizationValueToLocalizationValueModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<LocalizationValue, LocalizationValueModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.NewValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}