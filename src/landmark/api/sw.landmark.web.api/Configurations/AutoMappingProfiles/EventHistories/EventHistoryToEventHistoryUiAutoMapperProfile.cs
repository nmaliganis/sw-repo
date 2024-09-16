using AutoMapper;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.landmark.model;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.EventHistories
{
    internal class EventHistoryToEventHistoryUiAutoMapperProfile : Profile
    {
        public EventHistoryToEventHistoryUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<EventHistory, EventHistoryUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src => src.SensorId))
                .ForMember(dest => dest.EventPositionId, opt => opt.MapFrom(src => src.EventPositionId))
                .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => src.Recorded))
                .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.Received))
                .ForMember(dest => dest.EventValue, opt => opt.MapFrom(src => src.EventValue))
                .ForMember(dest => dest.EventValueJson, opt => opt.MapFrom(src => src.EventValueJson.RootElement.ToString()))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
