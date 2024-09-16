using AutoMapper;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.model.Geofence;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Geofences
{
    internal class GeofenceToGeofenceModificationUiAutoMapperProfile : Profile
    {
        public GeofenceToGeofenceModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Geofence, GeofenceModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
