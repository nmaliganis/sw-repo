using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.ItineraryTemplates;
using sw.infrastructure.Extensions;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.ItineraryTemplates;

internal class ItineraryTemplateToItineraryTemplateUiAutoMapperProfile : Profile
{
    public ItineraryTemplateToItineraryTemplateUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<ItineraryTemplate, ItineraryTemplateUiModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Occurrence, opt => opt.MapFrom(src => src.Occurrence.ObjectToJson<OccurrenceJsonbType>()))
            .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones.ObjectToJson<ZoneJsonbType>()))
            .ForMember(dest => dest.StartTime, opt => opt
                .MapFrom(src => src.StartTime))
            .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.Stream))
            .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.MinFillLevel, opt => opt.MapFrom(src => src.MinFillLevel))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}