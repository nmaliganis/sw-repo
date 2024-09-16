using AutoMapper;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Persons
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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.ExtPhone, opt => opt.MapFrom(src => src.ExtPhone))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.AddressStreet1, opt => opt.MapFrom(src => src.AddressStreet1))
                .ForMember(dest => dest.AddressStreet2, opt => opt.MapFrom(src => src.AddressStreet2))
                .ForMember(dest => dest.AddressPostCode, opt => opt.MapFrom(src => src.AddressPostCode))
                .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.AddressCity))
                .ForMember(dest => dest.AddressRegion, opt => opt.MapFrom(src => src.AddressRegion))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.ExtMobile, opt => opt.MapFrom(src => src.ExtMobile))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PersonRoleId, opt => opt.MapFrom(src => src.PersonRoleId))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
