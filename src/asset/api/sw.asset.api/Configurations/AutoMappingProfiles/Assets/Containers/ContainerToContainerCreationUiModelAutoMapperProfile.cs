using AutoMapper;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.model.Assets.Containers;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Assets.Containers;

internal class ContainerToContainerCreationUiModelAutoMapperProfile : Profile
{
    public ContainerToContainerCreationUiModelAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Container, ContainerCreationUiModel>()
            // Asset
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))

            // Container
            .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.GeoPoint.Coordinate.Y))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.GeoPoint.Coordinate.X))
            .ForMember(dest => dest.TimeToFull, opt => opt.MapFrom(src => src.TimeToFull))
            .ForMember(dest => dest.LastServicedDate, opt => opt.MapFrom(src => src.LastServicedDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ContainerStatus))
            .ForMember(dest => dest.MandatoryPickupDate, opt => opt.MapFrom(src => src.MandatoryPickupDate))
            .ForMember(dest => dest.MandatoryPickupActive, opt => opt.MapFrom(src => src.MandatoryPickupActive))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.WasteType, opt => opt.MapFrom(src => src.WasteType))
            .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Material))

            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ReverseMap()
            .MaxDepth(1)
            ;
    }
}