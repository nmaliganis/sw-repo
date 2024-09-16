using AutoMapper;
using sw.common.dtos.Vms.Departments;
using sw.auth.model.Departments;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Departments
{
    internal class DepartmentToDepartmentCreationUiAutoMapperProfile : Profile
    {
        public DepartmentToDepartmentCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Department, DepartmentCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
