using AutoMapper;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Companies
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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
