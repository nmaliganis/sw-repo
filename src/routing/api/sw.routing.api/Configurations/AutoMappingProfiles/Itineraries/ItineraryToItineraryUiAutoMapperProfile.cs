using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.model.Itineraries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Itineraries;

internal class ItineraryToItineraryUiAutoMapperProfile : Profile
{
    public ItineraryToItineraryUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Itinerary, ItineraryUiModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DriverTransportCombination, opt => opt.MapFrom(src => src.DriverTransportCombination))
            .ForMember(dest => dest.Template, opt => opt.MapFrom(src => src.Template))
            .ForMember(dest => dest.Jobs, opt => opt.MapFrom(src => src.Jobs))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}