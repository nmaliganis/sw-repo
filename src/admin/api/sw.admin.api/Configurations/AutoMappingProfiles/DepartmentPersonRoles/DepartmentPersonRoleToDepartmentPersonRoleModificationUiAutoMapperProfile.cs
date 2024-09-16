using AutoMapper;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.DepartmentPersonRoles
{
    internal class DepartmentPersonRoleToDepartmentPersonRoleModificationUiAutoMapperProfile : Profile
    {
        public DepartmentPersonRoleToDepartmentPersonRoleModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<DepartmentPersonRole, DepartmentPersonRoleModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
