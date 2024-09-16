using AutoMapper;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.landmark.model;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.EventPositions
{
    internal class EventPositionToEventPositionUiAutoMapperProfile : Profile
    {
        public EventPositionToEventPositionUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<EventPosition, EventPositionUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Position.Coordinate.Y))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Position.Coordinate.X))
                .ForMember(dest => dest.GeocodedPositionId, opt => opt.MapFrom(src => src.GeocodedPositionId))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
