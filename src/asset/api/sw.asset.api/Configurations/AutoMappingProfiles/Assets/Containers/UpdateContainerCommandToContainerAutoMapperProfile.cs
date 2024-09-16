using AutoMapper;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.model.Assets.Containers;
using NetTopologySuite.Geometries;

namespace sw.asset.api.Configurations.AutoMappingProfiles.Assets.Containers;

internal class UpdateContainerCommandToContainerAutoMapperProfile : Profile
{
    public UpdateContainerCommandToContainerAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<UpdateContainerCommand, Container>()
            // Asset
            //.ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.parameters.CompanyId))
            //.ForMember(dest => dest.AssetCategoryId, opt => opt.MapFrom(src => src.parameters.AssetCategoryId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parameters.Name))
            .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.Parameters.CodeErp))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Parameters.Image))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Parameters.Description))

            // Container
            .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.Parameters.IsVisible))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Parameters.Level))
            //.ForMember(dest => dest.Point, opt => opt.MapFrom<UpdatePointResolver>())
            .ForMember(dest => dest.TimeToFull, opt => opt.MapFrom(src => src.Parameters.TimeToFull))
            .ForMember(dest => dest.LastServicedDate, opt => opt.MapFrom(src => src.Parameters.LastServicedDate))
            .ForMember(dest => dest.ContainerStatus, opt => opt.MapFrom(src => src.Parameters.Status))
            .ForMember(dest => dest.MandatoryPickupDate, opt => opt.MapFrom(src => src.Parameters.MandatoryPickupDate))
            .ForMember(dest => dest.MandatoryPickupActive, opt => opt.MapFrom(src => src.Parameters.MandatoryPickupActive))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Parameters.Capacity))
            .ForMember(dest => dest.WasteType, opt => opt.MapFrom(src => src.Parameters.WasteType))
            .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Parameters.Material))
            .ReverseMap()
            .MaxDepth(1);
    }
}

internal class UpdatePointResolver : IValueResolver<UpdateContainerCommand, Container, Point>
{
    public Point Resolve(UpdateContainerCommand source, Container destination, Point member, ResolutionContext context)
    {
        return new Point(source.Parameters.Longitude, source.Parameters.Latitude)
        {
            SRID = 4326
        };
    }
}