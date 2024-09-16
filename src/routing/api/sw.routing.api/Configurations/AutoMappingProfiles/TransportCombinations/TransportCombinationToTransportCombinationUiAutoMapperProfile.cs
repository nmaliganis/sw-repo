using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.TransportCombination;
using sw.routing.model.Itineraries;
using sw.routing.model.TransportCombinations;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.TransportCombinations;

internal class TransportCombinationToTransportCombinationUiAutoMapperProfile : Profile
{
    public TransportCombinationToTransportCombinationUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<TransportCombination, TransportCombinationUiModel>()
            .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.Vehicles))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}