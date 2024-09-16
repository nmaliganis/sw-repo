using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.model;
using NetTopologySuite.Geometries;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.GeocodedPositions
{
    internal class CreateGeocodedPositionCommandToGeocodedPositionAutoMapperProfile : Profile
    {
        public CreateGeocodedPositionCommandToGeocodedPositionAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateGeocodedPositionCommand, GeocodedPosition>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom<CreatePointResolver>())
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Parameters.Street))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Parameters.Number))
                .ForMember(dest => dest.CrossStreet, opt => opt.MapFrom(src => src.Parameters.CrossStreet))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Parameters.City))
                .ForMember(dest => dest.Prefecture, opt => opt.MapFrom(src => src.Parameters.Prefecture))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Parameters.Country))
                .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.Parameters.Zipcode))
                .ForMember(dest => dest.GeocoderProfileId, opt => opt.MapFrom(src => src.Parameters.GeocoderProfileId))
                .ForMember(dest => dest.LastGeocoded, opt => opt.MapFrom(src => src.Parameters.LastGeocoded))

                .MaxDepth(1);
        }

        internal class CreatePointResolver : IValueResolver<CreateGeocodedPositionCommand, GeocodedPosition, Point>
        {
            public Point Resolve(CreateGeocodedPositionCommand source, GeocodedPosition destination, Point member, ResolutionContext context)
            {
                return new Point(source.Parameters.Longitude, source.Parameters.Latitude)
                {
                    SRID = 4326
                };
            }
        }
    }
}
