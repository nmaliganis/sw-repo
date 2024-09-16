using AutoMapper;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.model.Roles;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Roles
{
    internal class RoleToRoleModificationUiAutoMapperProfile : Profile
    {

        public RoleToRoleModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Role, RoleModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.Name))
                .MaxDepth(1);
        }
    }// Class: RoleToRoleModificationUiAutoMapperProfile
}// Namespace: sw.auth.api.Configurations.AutoMappingProfiles.Roles

