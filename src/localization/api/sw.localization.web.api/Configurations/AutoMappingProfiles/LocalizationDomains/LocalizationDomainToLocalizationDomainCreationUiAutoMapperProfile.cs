using AutoMapper;
using sw.localization.common.dtos.Vms.LocalizationDomains;
using sw.localization.model.Localizations;

namespace sw.localization.api.Configurations.AutoMappingProfiles.LocalizationDomains
{
    /// <summary>
    /// Class
    /// </summary>
    public class LocalizationDomainToLocalizationDomainCreationUiAutoMapperProfile : Profile
    {
        public LocalizationDomainToLocalizationDomainCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<LocalizationDomain, LocalizationDomainCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}