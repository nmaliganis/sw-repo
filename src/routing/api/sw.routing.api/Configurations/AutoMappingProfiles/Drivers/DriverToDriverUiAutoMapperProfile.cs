using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.Drivers;
using sw.routing.model.Drivers;
using sw.routing.model.Itineraries;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Drivers;

internal class DriverToDriverUiAutoMapperProfile : Profile
{
    public DriverToDriverUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Driver, DriverUiModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Member, opt => opt.MapFrom(src => src.Member))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}