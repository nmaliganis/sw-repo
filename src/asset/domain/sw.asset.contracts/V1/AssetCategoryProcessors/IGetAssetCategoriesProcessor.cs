using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetCategoryProcessors
{
    public interface IGetAssetCategoriesProcessor
    {
        Task<BusinessResult<PagedList<AssetCategoryUiModel>>> GetAssetCategoriesAsync(GetAssetCategoriesQuery qry);
    }
}
