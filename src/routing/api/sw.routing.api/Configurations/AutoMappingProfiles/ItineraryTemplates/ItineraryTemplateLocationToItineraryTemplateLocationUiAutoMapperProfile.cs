using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.model.Itineraries;
using sw.routing.model.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.ItineraryTemplates;

internal class ItineraryTemplateLocationToItineraryTemplateLocationUiAutoMapperProfile : Profile
{
    public ItineraryTemplateLocationToItineraryTemplateLocationUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<ItineraryTemplateLocationPoint, ItineraryTemplateLocationUiModel>()
            .ForMember(dest => dest.Location, opt => opt
                .MapFrom(src => src.Location))
            .ForMember(dest => dest.IsStart, opt => opt
                .MapFrom(src => src.IsStart))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}