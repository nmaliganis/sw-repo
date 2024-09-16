using AutoMapper;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.model.Assets.Containers;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Assets.Containers;

internal class ContainerToContainerUiAutoMapperProfile : Profile
{
    public ContainerToContainerUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Container, ContainerUiModel>()
            // Asset
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))

            // Container
            .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(dest => dest.PrevLevel, opt => opt.MapFrom(src => src.PrevLevel))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.GeoPoint.Coordinate.Y))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.GeoPoint.Coordinate.X))
            .ForMember(dest => dest.TimeToFull, opt => opt.MapFrom(src => src.TimeToFull))
            .ForMember(dest => dest.LastServicedDate, opt => opt.MapFrom(src => src.LastServicedDate))
            .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.ModifiedDate))
            .ForMember(dest => dest.MandatoryPickupDate, opt => opt.MapFrom(src => src.MandatoryPickupDate))
            .ForMember(dest => dest.MandatoryPickupActive, opt => opt.MapFrom(src => src.MandatoryPickupActive))

            // Container
            .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.Zone.Id))
            .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.Zone.Name))

            .ForMember(dest => dest.ContainerStatus, opt => opt.MapFrom(src => src.ContainerStatus))
            .ForMember(dest => dest.ContainerStatusLocale, opt => opt.MapFrom(src => src.ContainerStatus.ToString()))

            .ForMember(dest => dest.BinStatus, opt => opt.MapFrom(src => src.ContainerCondition))
            .ForMember(dest => dest.BinStatusLocale, opt => opt.MapFrom(src => src.ContainerCondition.ToString()))

            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.CapacityLocale, opt => opt.MapFrom(src => src.Capacity.ToString()))

            .ForMember(dest => dest.WasteType, opt => opt.MapFrom(src => src.WasteType))
            .ForMember(dest => dest.WasteTypeLocale, opt => opt.MapFrom(src => src.WasteType.ToString()))

            .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Material))
            .ForMember(dest => dest.MaterialLocale, opt => opt.MapFrom(src => src.Material.ToString()))
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .MaxDepth(1)
            .ReverseMap()
            ;
    }
}