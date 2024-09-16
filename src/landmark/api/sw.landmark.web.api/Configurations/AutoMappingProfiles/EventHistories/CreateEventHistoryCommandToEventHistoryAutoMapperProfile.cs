using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.model;
using System.Text.Json;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.EventHistories
{
    internal class CreateEventHistoryCommandToEventHistoryAutoMapperProfile : Profile
    {
        public CreateEventHistoryCommandToEventHistoryAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<CreateEventHistoryCommand, EventHistory>()
                .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src => src.Parameters.SensorId))
                .ForMember(dest => dest.EventPositionId, opt => opt.MapFrom(src => src.Parameters.EventPositionId))
                .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => src.Parameters.Recorded))
                .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.Parameters.Received))
                .ForMember(dest => dest.EventValue, opt => opt.MapFrom(src => src.Parameters.EventValue))
                .ForMember(dest => dest.EventValueJson, opt => opt.MapFrom<CreateJsonResolver>())

                .MaxDepth(1);
        }

        internal class CreateJsonResolver : IValueResolver<CreateEventHistoryCommand, EventHistory, JsonDocument>
        {
            public JsonDocument Resolve(CreateEventHistoryCommand source, EventHistory destination, JsonDocument member, ResolutionContext context)
            {
                return JsonDocument.Parse(source.Parameters.EventValueJson);
            }
        }
    }
}
