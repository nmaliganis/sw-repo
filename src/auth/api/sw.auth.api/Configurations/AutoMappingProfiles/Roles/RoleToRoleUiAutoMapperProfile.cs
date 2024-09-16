using AutoMapper;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.model.Roles;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Roles
{
    internal class RoleToRoleUiAutoMapperProfile : Profile
    {
        public RoleToRoleUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Role, RoleUiModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
