using AutoMapper;
using sw.auth.model.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;

namespace sw.onboarding.api.Configurations.AutoMappingProfiles.Roles
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
