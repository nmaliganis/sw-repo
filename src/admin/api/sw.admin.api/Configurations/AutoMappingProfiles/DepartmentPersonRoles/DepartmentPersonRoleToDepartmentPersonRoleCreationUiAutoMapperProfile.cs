using AutoMapper;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.DepartmentPersonRoles
{
    internal class DepartmentPersonRoleToDepartmentPersonRoleCreationUiAutoMapperProfile : Profile
    {
        public DepartmentPersonRoleToDepartmentPersonRoleCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<DepartmentPersonRole, DepartmentPersonRoleCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
