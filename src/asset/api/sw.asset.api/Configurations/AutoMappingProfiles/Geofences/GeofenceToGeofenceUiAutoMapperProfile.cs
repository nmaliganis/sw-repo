using AutoMapper;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.model;
using sw.asset.model.Geofence;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Geofences;

internal class GeofenceToGeofenceUiAutoMapperProfile : Profile
{
    public GeofenceToGeofenceUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Geofence, GeofenceUiModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key))
            .ForMember(dest => dest.GeoPoints, opt => opt.MapFrom(src => src.GeoPoints))
            .ForMember(dest => dest.PointId, opt => opt.MapFrom(src => src.PointId))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .MaxDepth(1);
    }
}// Class: GeofenceToGeofenceUiAutoMapperProfile
