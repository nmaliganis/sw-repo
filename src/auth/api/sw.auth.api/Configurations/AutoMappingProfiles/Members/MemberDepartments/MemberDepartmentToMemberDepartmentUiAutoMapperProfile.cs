using AutoMapper;
using sw.auth.common.dtos.Vms.Members;
using sw.auth.common.dtos.Vms.Members.MemberDepartments;
using sw.auth.model.Members;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Members.MemberDepartments;

internal class MemberDepartmentToMemberDepartmentUiAutoMapperProfile : Profile
{
    public MemberDepartmentToMemberDepartmentUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<MemberDepartment, MemberDepartmentUiModel>()
            .ForMember(dest => dest.Member, opt => opt.MapFrom(src => src.Member))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}//Class : MemberDepartmentToMemberDepartmentUiAutoMapperProfile