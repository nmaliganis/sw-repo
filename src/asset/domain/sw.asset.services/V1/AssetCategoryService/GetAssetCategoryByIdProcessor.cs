using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetCategoryService
{
    public class GetAssetCategoryByIdProcessor :
        IGetAssetCategoryByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        public GetAssetCategoryByIdProcessor(IAssetCategoryRepository assetCategoryRepository, IAutoMapper autoMapper)
        {
            _assetCategoryRepository = assetCategoryRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<AssetCategoryUiModel>> GetAssetCategoryByIdAsync(long id)
        {
            var bc = new BusinessResult<AssetCategoryUiModel>(new AssetCategoryUiModel());

            var assetCategory = _assetCategoryRepository.FindActiveById(id);
            if (assetCategory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "AssetCategory Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<AssetCategoryUiModel>(assetCategory);
            response.Message = $"AssetCategory id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
