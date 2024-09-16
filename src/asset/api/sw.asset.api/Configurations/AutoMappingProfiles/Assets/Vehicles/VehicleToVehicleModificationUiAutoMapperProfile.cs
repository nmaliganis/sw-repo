using AutoMapper;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.model.Assets.Vehicles;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Assets.Vehicles
{
    internal class VehicleToVehicleModificationUiAutoMapperProfile : Profile
    {
        public VehicleToVehicleModificationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Vehicle, VehicleModificationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
