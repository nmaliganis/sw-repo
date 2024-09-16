using AutoMapper;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.model;
using sw.asset.model.Companies;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Companies
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
