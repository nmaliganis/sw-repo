using AutoMapper;
using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.model.Events;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Events;

internal class UpdateEventHistoryCommandToEventHistoryAutoMapperProfile : Profile
{
  public UpdateEventHistoryCommandToEventHistoryAutoMapperProfile()
  {
    ConfigureMapping();
  }

  public void ConfigureMapping()
  {
    CreateMap<UpdateEventHistoryCommand, EventHistory>()
      .ForMember(dest => dest.EventValue, opt => opt.MapFrom(src => src.Parameters.EventValueJson))
      .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.Parameters.Received))
      .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => src.Parameters.Recorded))
      .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Parameters.EventValue))
      .ReverseMap()
      .MaxDepth(1);
  }
}