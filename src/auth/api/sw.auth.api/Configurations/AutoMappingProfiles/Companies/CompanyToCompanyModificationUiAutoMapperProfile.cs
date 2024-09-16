using AutoMapper;
using sw.auth.model.Companies;
using sw.common.dtos.Vms.Companies;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Companies
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
                .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ModifiedDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ModifiedCodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }

    
}
