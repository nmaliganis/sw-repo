using AutoMapper;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.model;
using sw.asset.model.Assets.Categories;

namespace sw.asset.api.Configurations.AutoMappingProfiles.AssetCategories
{
    internal class AssetCategoryToAssetCategoryCreationUiAutoMapperProfile : Profile
    {
        public AssetCategoryToAssetCategoryCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<AssetCategory, AssetCategoryCreationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1);
        }
    }
}
