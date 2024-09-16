using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.Vehicles;
using sw.routing.model.Itineraries;
using sw.routing.model.Vehicles;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Vehicles;

internal class VehicleTransportCombinationToVehicleTransportCombinationUiAutoMapperProfile : Profile
{
    public VehicleTransportCombinationToVehicleTransportCombinationUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<VehicleTransportCombination, VehicleTransportCombinationUiModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}