using AutoMapper;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.model.Events;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Events;

internal class EventHistoryToEventHistoryUiAutoMapperProfile : Profile
{
    public EventHistoryToEventHistoryUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<EventHistory, EventHistoryUiModel>()
          .ForMember(dest => dest.EventValueJson, opt => opt.MapFrom(src => src.EventValue.Params))
          .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.Received))
          .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => src.Recorded))
          .ForMember(dest => dest.EventValue, opt => opt.MapFrom(src => src.Value))
          .ForMember(dest => dest.IsWastePickUp, opt => opt.MapFrom(src => src.Sensor.SensorType.SensorTypeIndex == 18))
          .ForMember(dest => dest.Message, opt => opt.Ignore())
          .ReverseMap()
          .MaxDepth(1);
    }
}