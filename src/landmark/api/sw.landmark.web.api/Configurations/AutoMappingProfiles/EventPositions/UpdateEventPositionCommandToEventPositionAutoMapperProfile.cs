using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.model;
using NetTopologySuite.Geometries;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.EventPositions
{
    internal class UpdateEventPositionCommandToEventPositionAutoMapperProfile : Profile
    {
        public UpdateEventPositionCommandToEventPositionAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateEventPositionCommand, EventPosition>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom<CreatePointResolver>())
                .ForMember(dest => dest.GeocodedPositionId, opt => opt.MapFrom(src => src.Parameters.GeocodedPositionId))

                .MaxDepth(1);
        }

        internal class CreatePointResolver : IValueResolver<UpdateEventPositionCommand, EventPosition, Point>
        {
            public Point Resolve(UpdateEventPositionCommand source, EventPosition destination, Point member, ResolutionContext context)
            {
                return new Point(source.Parameters.Longitude, source.Parameters.Latitude)
                {
                    SRID = 4326
                };
            }
        }
    }
}
