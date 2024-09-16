using AutoMapper;
using sw.auth.common.dtos.Vms.Companies;
using sw.auth.model.Companies;
using sw.common.dtos.Vms.Companies;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Companies
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
                .ReverseMap()
                .MaxDepth(1);
        }
    }
}
