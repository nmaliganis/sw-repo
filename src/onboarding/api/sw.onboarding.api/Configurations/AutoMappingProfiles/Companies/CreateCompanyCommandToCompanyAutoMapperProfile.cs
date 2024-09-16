using AutoMapper;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.onboarding.model.Companies;

namespace sw.onboarding.api.Configurations.AutoMappingProfiles.Companies
{
    internal class CreateCompanyCommandToCompanyAutoMapperProfile : Profile
    {
        public CreateCompanyCommandToCompanyAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateCompanyCommand, Company>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .MaxDepth(1);
        }
    }
}
