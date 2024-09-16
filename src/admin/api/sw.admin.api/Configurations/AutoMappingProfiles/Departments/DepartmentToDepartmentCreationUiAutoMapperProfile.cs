using AutoMapper;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Departments
{
    internal class DepartmentToDepartmentCreationUiAutoMapperProfile : Profile
    {
        public DepartmentToDepartmentCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Department, DepartmentCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
