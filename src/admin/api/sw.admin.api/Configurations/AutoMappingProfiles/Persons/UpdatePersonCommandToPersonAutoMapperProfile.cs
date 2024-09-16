using AutoMapper;
using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.model;

namespace sw.admin.api.Configurations.AutoMappingProfiles.Persons
{
    internal class UpdatePersonCommandToPersonAutoMapperProfile : Profile
    {
        public UpdatePersonCommandToPersonAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdatePersonCommand, Person>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Parameters.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Parameters.LastName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Parameters.Gender))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Parameters.Phone))
                .ForMember(dest => dest.ExtPhone, opt => opt.MapFrom(src => src.Parameters.ExtPhone))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Parameters.Notes))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Parameters.Email))
                .ForMember(dest => dest.AddressStreet1, opt => opt.MapFrom(src => src.Parameters.AddressStreet1))
                .ForMember(dest => dest.AddressStreet2, opt => opt.MapFrom(src => src.Parameters.AddressStreet2))
                .ForMember(dest => dest.AddressPostCode, opt => opt.MapFrom(src => src.Parameters.AddressPostCode))
                .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.Parameters.AddressCity))
                .ForMember(dest => dest.AddressRegion, opt => opt.MapFrom(src => src.Parameters.AddressRegion))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Parameters.Mobile))
                .ForMember(dest => dest.ExtMobile, opt => opt.MapFrom(src => src.Parameters.ExtMobile))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Parameters.Status))
                .ForMember(dest => dest.PersonRoleId, opt => opt.MapFrom(src => src.Parameters.PersonRoleId))

                .MaxDepth(1);
        }
    }
}
