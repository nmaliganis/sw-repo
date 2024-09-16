using AutoMapper;
using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Departments
{
    internal class CreateDepartmentCommandToDepartmentAutoMapperProfile : Profile
    {
        public CreateDepartmentCommandToDepartmentAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateDepartmentCommand, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Parameters.Notes))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Parameters.CompanyId))

                .MaxDepth(1);
        }
    }
}
