using AutoMapper;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.model.Assets.Vehicles;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Assets.Vehicles
{
    internal class VehicleToVehicleUiAutoMapperProfile : Profile
    {
        public VehicleToVehicleUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Vehicle, VehicleUiModel>()
                // Asset
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TypeValue, opt => opt.MapFrom(src => src.Company.Id))
                .ForMember(dest => dest.AssetCategoryId, opt => opt.MapFrom(src => src.AssetCategory.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))

                // Vehicle
                .ForMember(dest => dest.NumPlate, opt => opt.MapFrom(src => src.NumPlate))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.RegisteredDate, opt => opt.MapFrom(src => src.RegisteredDate))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.TypeValue, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusValue, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Gas, opt => opt.MapFrom(src => src.Gas))
                .ForMember(dest => dest.GasValue, opt => opt.MapFrom(src => src.Gas.ToString()))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Axels, opt => opt.MapFrom(src => src.Axels))
                .ForMember(dest => dest.MinTurnRadius, opt => opt.MapFrom(src => src.MinTurnRadius))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))

                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ReverseMap()
                .MaxDepth(1);
        }
    }
}
