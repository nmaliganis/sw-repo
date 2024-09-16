using AutoMapper;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.landmark.model;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.GeocodedPositions
{
    internal class GeocodedPositionToGeocodedPositionCreationUiModelAutoMapperProfile : Profile
    {
        public GeocodedPositionToGeocodedPositionCreationUiModelAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<GeocodedPosition, GeocodedPositionCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Position.Coordinate.Y))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Position.Coordinate.X))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CrossStreet, opt => opt.MapFrom(src => src.CrossStreet))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Prefecture, opt => opt.MapFrom(src => src.Prefecture))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.Zipcode))
                .ForMember(dest => dest.GeocoderProfileId, opt => opt.MapFrom(src => src.GeocoderProfileId))
                .ForMember(dest => dest.LastGeocoded, opt => opt.MapFrom(src => src.LastGeocoded))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
