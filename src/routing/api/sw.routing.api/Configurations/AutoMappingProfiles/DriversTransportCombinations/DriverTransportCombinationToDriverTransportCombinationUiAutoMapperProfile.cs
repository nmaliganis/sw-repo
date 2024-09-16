using AutoMapper;
using sw.routing.common.dtos.Vms.DriversTransportCombinations;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.routing.model.TransportCombinations;

namespace sw.routing.api.Configurations.AutoMappingProfiles.DriversTransportCombinations;

internal class DriverTransportCombinationToDriverTransportCombinationUiAutoMapperProfile : Profile
{
    public DriverTransportCombinationToDriverTransportCombinationUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<DriverTransportCombination, DriverTransportCombinationUiModel>()
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver))
            .ForMember(dest => dest.TransportCombination, opt => opt.MapFrom(src => src.TransportCombination))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}