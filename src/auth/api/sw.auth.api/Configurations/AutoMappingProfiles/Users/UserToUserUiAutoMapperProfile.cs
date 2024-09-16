using System.Linq;
using AutoMapper;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.model.Users;
using sw.common.dtos.Vms.Users;

namespace sw.auth.api.Configurations.AutoMappingProfiles.Users
{
    internal class UserToUserUiAutoMapperProfile : Profile
    {
        public UserToUserUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<User, UserUiModel>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.IsActivated, opt => opt.MapFrom(src => src.IsActivated))
                .ForMember(dest => dest.ResetDate, opt => opt.MapFrom(src => src.ResetDate))
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
                .ForMember(dest => dest.IsLoggedIn, opt => opt.MapFrom(src => src.IsLoggedIn))
                .ForMember(dest => dest.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.Member.Id))
                .ForMember(dest => dest.MemberEmail, opt => opt.MapFrom(src => src.Member.Email))
                .ForMember(dest => dest.ResetKey, opt => opt.MapFrom(src => src.ResetKey))
                .ForMember(dest => dest.Member, opt => opt.MapFrom(src => src.Member))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ReverseMap()
                .MaxDepth(1);
        }
    }
}
