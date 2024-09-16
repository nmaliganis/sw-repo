using AutoMapper;
using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Companies
{
    internal class UpdateCompanyCommandToCompanyAutoMapperProfile : Profile
    {
        public UpdateCompanyCommandToCompanyAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateCompanyCommand, Company>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))

                .MaxDepth(1);
        }
    }
}
