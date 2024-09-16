using AutoMapper;
using sw.asset.common.dtos.Vms.Persons;
using sw.asset.model.Persons;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Persons
{
    internal class PersonToPersonUiAutoMapperProfile : Profile
    {
        public PersonToPersonUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Person, PersonUiModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                //.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.PersonRoles.Select(x => x.Role).ToList()))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
