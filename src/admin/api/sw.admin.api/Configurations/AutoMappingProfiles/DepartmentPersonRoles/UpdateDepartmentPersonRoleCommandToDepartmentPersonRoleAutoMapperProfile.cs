using AutoMapper;
using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.DepartmentPersonRoles
{
    internal class UpdateDepartmentPersonRoleCommandToDepartmentPersonRoleAutoMapperProfile : Profile
    {
        public UpdateDepartmentPersonRoleCommandToDepartmentPersonRoleAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateDepartmentPersonRoleCommand, DepartmentPersonRole>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Parameters.Notes))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Parameters.DepartmentId))

                .MaxDepth(1);
        }
    }
}
