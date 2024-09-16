using AutoMapper;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.model;
using sw.asset.model.Devices;
using StackExchange.Redis;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Geofences
{
    internal class GeoEntryToGeoEntryUiAutoMapperProfile : Profile
    {
        public GeoEntryToGeoEntryUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<GeoEntry, GeoEntryUiModel>()
                //.ForMember(dest => dest.DeviceModelId, opt => opt.MapFrom(src => src.DeviceModelId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Member))
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Lon, opt => opt.MapFrom(src => src.Longitude))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
