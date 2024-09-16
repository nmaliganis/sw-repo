using AutoMapper;
using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.model.Assets.Vehicles;
using NetTopologySuite.Geometries;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Assets.Vehicles
{
    internal class UpdateVehicleCommandToVehicleAutoMapperProfile : Profile
    {
        public UpdateVehicleCommandToVehicleAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<UpdateVehicleCommand, Vehicle>()
                // Asset
                //.ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.parameters.CompanyId))
                //.ForMember(dest => dest.AssetCategoryId, opt => opt.MapFrom(src => src.parameters.AssetCategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.parameters.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.parameters.CodeErp))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.parameters.Image))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.parameters.Description))

                // Vehicle
                .ForMember(dest => dest.NumPlate, opt => opt.MapFrom(src => src.parameters.NumPlate))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.parameters.Brand))
                .ForMember(dest => dest.RegisteredDate, opt => opt.MapFrom(src => src.parameters.RegisteredDate))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.parameters.Type))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.parameters.Status))
                .ForMember(dest => dest.Gas, opt => opt.MapFrom(src => src.parameters.Gas))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.parameters.Height))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.parameters.Width))
                .ForMember(dest => dest.Axels, opt => opt.MapFrom(src => src.parameters.Axels))
                .ForMember(dest => dest.MinTurnRadius, opt => opt.MapFrom(src => src.parameters.MinTurnRadius))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.parameters.Length))

                .MaxDepth(1);
        }
    }
}
