using AutoMapper;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Persons
{
    internal class PersonToPersonCreationUiAutoMapperProfile : Profile
    {
        public PersonToPersonCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Person, PersonCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
