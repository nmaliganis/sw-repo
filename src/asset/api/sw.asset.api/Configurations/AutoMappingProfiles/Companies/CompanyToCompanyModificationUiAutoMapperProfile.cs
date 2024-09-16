using AutoMapper;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.model;
using sw.asset.model.Companies;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Companies
{
    internal class CompanyToCompanyModificationUiAutoMapperProfile : Profile
    {
        public CompanyToCompanyModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Company, CompanyModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ReverseMap()
                .MaxDepth(1);
        }
    }
}
