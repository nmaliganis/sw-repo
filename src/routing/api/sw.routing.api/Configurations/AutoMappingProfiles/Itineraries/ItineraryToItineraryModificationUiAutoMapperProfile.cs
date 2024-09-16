using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.model.Itineraries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Itineraries;

internal class ItineraryToItineraryModificationUiAutoMapperProfile : Profile
{

    public ItineraryToItineraryModificationUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Itinerary, ItineraryModificationUiModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.Name))
            .ReverseMap()
            .MaxDepth(1);
    }
}
