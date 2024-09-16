using AutoMapper;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.model;
using sw.asset.model.Assets.Categories;

namespace sw.asset.api.Configurations.AutoMappingProfiles.AssetCategories
{
    internal class AssetCategoryToAssetCategoryUiAutoMapperProfile : Profile
    {
        public AssetCategoryToAssetCategoryUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<AssetCategory, AssetCategoryUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CodeErp, opt => opt.MapFrom(src => src.CodeErp))
                //.ForMember(dest => dest.Params, opt => opt.MapFrom(src => src.Params.RootElement.ToString()))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
