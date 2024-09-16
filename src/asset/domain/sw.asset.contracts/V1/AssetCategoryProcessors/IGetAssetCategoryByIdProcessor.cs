using sw.asset.common.dtos.Vms.AssetCategories;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetCategoryProcessors
{
    public interface IGetAssetCategoryByIdProcessor
    {
        Task<BusinessResult<AssetCategoryUiModel>> GetAssetCategoryByIdAsync(long id);
    }
}
