using AutoMapper;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.model;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Departments
{
    internal class DepartmentToDepartmentUiAutoMapperProfile : Profile
    {
        public DepartmentToDepartmentUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Department, DepartmentUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
