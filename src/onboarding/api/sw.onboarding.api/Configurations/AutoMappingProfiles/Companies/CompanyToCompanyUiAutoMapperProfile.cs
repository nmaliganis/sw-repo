using AutoMapper;
using sw.auth.common.dtos.Vms.Companies;
using sw.onboarding.model.Companies;

namespace sw.onboarding.api.Configurations.AutoMappingProfiles.Companies
{
    internal class CompanyToCompanyUiAutoMapperProfile : Profile
    {
        public CompanyToCompanyUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Company, CompanyUiModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
