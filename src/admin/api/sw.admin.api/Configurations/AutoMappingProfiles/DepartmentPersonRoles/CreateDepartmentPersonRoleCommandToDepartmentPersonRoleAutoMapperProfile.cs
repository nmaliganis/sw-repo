using AutoMapper;
using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.DepartmentPersonRoles
{
    internal class CreateDepartmentPersonRoleCommandToDepartmentPersonRoleAutoMapperProfile : Profile
    {
        public CreateDepartmentPersonRoleCommandToDepartmentPersonRoleAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateDepartmentPersonRoleCommand, DepartmentPersonRole>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Parameters.Notes))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Parameters.DepartmentId))

                .MaxDepth(1);
        }
    }
}
