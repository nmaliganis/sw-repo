using System.Linq;
using AutoMapper;
using sw.auth.common.dtos.Vms.Members;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.model.Members;
using sw.auth.model.Users;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Members
{
    internal class MemberToMemberUiAutoMapperProfile : Profile
    {
        public MemberToMemberUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Member, MemberUiModel>()
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
                .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments.ToList()))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ReverseMap()
                .MaxDepth(1);
        }
    }
}
