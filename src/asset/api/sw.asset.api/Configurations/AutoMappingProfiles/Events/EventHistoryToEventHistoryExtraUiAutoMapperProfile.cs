using AutoMapper;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.model.Events;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Events;

internal class EventHistoryToEventHistoryExtraUiAutoMapperProfile : Profile
{
    public EventHistoryToEventHistoryExtraUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<EventHistory, EventHistoryExtraUiModel>()
          .ForMember(dest => dest.EventValueJson, opt => opt.MapFrom(src => src.EventValue.Params))
          .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.Received))
          .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => src.Recorded))
          .ForMember(dest => dest.EventValue, opt => opt.MapFrom(src => src.Value))
          .ForMember(dest => dest.AssetId, opt => opt.MapFrom(src => src.Sensor.Asset.Id))
          .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Sensor.Device.Id))
          .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src => src.Sensor.Id))
          .ForMember(dest => dest.Message, opt => opt.Ignore())
          .ReverseMap()
          .MaxDepth(1);
    }
}