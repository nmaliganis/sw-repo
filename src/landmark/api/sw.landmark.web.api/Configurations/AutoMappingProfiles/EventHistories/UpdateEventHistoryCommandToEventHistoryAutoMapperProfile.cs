using AutoMapper;
using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.model;
using System.Text.Json;

namespace sw.landmark.web.api.Configurations.AutoMappingProfiles.EventHistories
{
    internal class UpdateEventHistoryCommandToEventHistoryAutoMapperProfile : Profile
    {
        public UpdateEventHistoryCommandToEventHistoryAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateEventHistoryCommand, EventHistory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src => src.Parameters.SensorId))
                .ForMember(dest => dest.EventPositionId, opt => opt.MapFrom(src => src.Parameters.EventPositionId))
                .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => src.Parameters.Recorded))
                .ForMember(dest => dest.Received, opt => opt.MapFrom(src => src.Parameters.Received))
                .ForMember(dest => dest.EventValue, opt => opt.MapFrom(src => src.Parameters.EventValue))
                .ForMember(dest => dest.EventValueJson, opt => opt.MapFrom<CreateJsonResolver>())

                .MaxDepth(1);
        }

        internal class CreateJsonResolver : IValueResolver<UpdateEventHistoryCommand, EventHistory, JsonDocument>
        {
            public JsonDocument Resolve(UpdateEventHistoryCommand source, EventHistory destination, JsonDocument member, ResolutionContext context)
            {
                return JsonDocument.Parse(source.Parameters.EventValueJson);
            }
        }
    }
}
