using AutoMapper;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.model.Itineraries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Itineraries;

internal class CreateItineraryCommandToItineraryAutoMapperProfile : Profile
{
    public CreateItineraryCommandToItineraryAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<CreateItineraryCommand, Itinerary>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
            .ReverseMap()
            .MaxDepth(1);
    }
}