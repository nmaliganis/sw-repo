using AutoMapper;
using sw.common.dtos.Vms.Departments;
using sw.auth.model.Departments;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Departments
{
    internal class DepartmentToDepartmentModificationUiAutoMapperProfile : Profile
    {
        public DepartmentToDepartmentModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Department, DepartmentModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ModifiedNotes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.ModifiedCodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
