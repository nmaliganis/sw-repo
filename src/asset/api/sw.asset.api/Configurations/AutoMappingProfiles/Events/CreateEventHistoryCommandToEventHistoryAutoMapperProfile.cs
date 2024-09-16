using AutoMapper;
using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.model.Events;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Events;

internal class CreateEventHistoryCommandToEventHistoryAutoMapperProfile : Profile
{
    public CreateEventHistoryCommandToEventHistoryAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<CreateEventHistoryCommand, EventHistory>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Parameters.EventValue))
            .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.Parameters.Received))
            .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => src.Parameters.Recorded))
            .ReverseMap()
            .MaxDepth(1);
    }
}
