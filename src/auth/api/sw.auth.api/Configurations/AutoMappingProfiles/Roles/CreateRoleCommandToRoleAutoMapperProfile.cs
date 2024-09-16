using AutoMapper;
using sw.auth.common.dtos.Cqrs.Roles;
using sw.auth.model.Roles;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Roles
{
    internal class CreateRoleCommandToRoleAutoMapperProfile : Profile
    {
        public CreateRoleCommandToRoleAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateRoleCommand, Role>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .MaxDepth(1);
        }
    }
}
