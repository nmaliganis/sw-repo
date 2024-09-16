using AutoMapper;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.model;
using sw.asset.model.Companies;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Companies
{
    internal class CompanyToCompanyCreationUiAutoMapperProfile : Profile
    {
        public CompanyToCompanyCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Company, CompanyCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
