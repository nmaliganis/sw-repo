using AutoMapper;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.model.Events;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Events;

internal class EventHistoryToEventHistoryCreationUiModelAutoMapperProfile : Profile
{
    public EventHistoryToEventHistoryCreationUiModelAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<EventHistory, EventHistoryCreationUiModel>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.Message, opt => opt.Ignore())
          .ReverseMap()
          .MaxDepth(1);
    }
}