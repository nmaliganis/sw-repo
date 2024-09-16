using AutoMapper;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.model.Localizations;

namespace sw.localization.api.Configurations.AutoMappingProfiles.LocalizationLanguages
{
    /// <summary>
    /// Class
    /// </summary>
    public class LocalizationLanguageToLocalizationLanguageCreationUiAutoMapperProfile : Profile
    {
        public LocalizationLanguageToLocalizationLanguageCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<LocalizationLanguage, LocalizationLanguageCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}