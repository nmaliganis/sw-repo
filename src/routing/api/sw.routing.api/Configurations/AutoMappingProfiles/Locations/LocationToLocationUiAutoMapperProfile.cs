using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.model.Itineraries;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Locations;

internal class LocationToLocationUiAutoMapperProfile : Profile
{
    public LocationToLocationUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<LocationPoint, LocationUiModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Lon, opt => opt.MapFrom(src => src.Location.Coordinate.X))
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Location.Coordinate.Y))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}