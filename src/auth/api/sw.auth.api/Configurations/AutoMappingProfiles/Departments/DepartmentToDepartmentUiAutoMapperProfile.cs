using System.Linq;
using AutoMapper;
using sw.auth.common.dtos.Vms.Departments;
using sw.common.dtos.Vms.Departments;
using sw.auth.model.Departments;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Departments
{
    internal class DepartmentToDepartmentUiAutoMapperProfile : Profile
    {
        public DepartmentToDepartmentUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Department, DepartmentUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.ToList()))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ReverseMap()
                .MaxDepth(1)
                ;
        }
    }
}
