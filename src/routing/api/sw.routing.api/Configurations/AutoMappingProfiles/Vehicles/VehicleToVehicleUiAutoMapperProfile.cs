using AutoMapper;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.common.dtos.Vms.Vehicles;
using sw.routing.model.Itineraries;
using sw.routing.model.Vehicles;
using GeoAPI.Geometries;

namespace sw.routing.api.Configurations.AutoMappingProfiles.Vehicles;

internal class VehicleToVehicleUiAutoMapperProfile : Profile
{
    public VehicleToVehicleUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Vehicle, VehicleUiModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NumPlate, opt => opt.MapFrom(src => src.NumPlate))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1);
    }
}