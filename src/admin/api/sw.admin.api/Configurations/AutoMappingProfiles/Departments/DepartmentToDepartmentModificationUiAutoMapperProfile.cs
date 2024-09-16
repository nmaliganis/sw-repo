using AutoMapper;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Departments
{
    internal class DepartmentToDepartmentModificationUiAutoMapperProfile : Profile
    {
        public DepartmentToDepartmentModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Department, DepartmentModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
