using AutoMapper;
using sw.auth.common.dtos.Vms.Departments.DepartmentRoles;
using sw.auth.model.Departments;
using sw.common.dtos.Vms.Departments;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Departments.DepartmentRoles;

internal class DepartmentRoleToDepartmentRoleUiAutoMapperProfile : Profile
{
    public DepartmentRoleToDepartmentRoleUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<DepartmentRole, DepartmentRoleUiModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}