using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Text.Json;
using System.Threading.Tasks;
using sw.asset.model.Assets.Categories;

namespace sw.asset.services.V1.AssetCategoryService
{
    public class UpdateAssetCategoryProcessor :
        IUpdateAssetCategoryProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateAssetCategoryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IAssetCategoryRepository assetCategoryRepository)
        {
            this._uOf = uOf;
            this._autoMapper = autoMapper;
            this._assetCategoryRepository = assetCategoryRepository;
        }

        public async Task<BusinessResult<AssetCategoryModificationUiModel>> UpdateAssetCategoryAsync(UpdateAssetCategoryCommand updateCommand)
        {
            var bc = new BusinessResult<AssetCategoryModificationUiModel>(new AssetCategoryModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var assetCategory = _assetCategoryRepository.FindBy(updateCommand.Id);
            if (assetCategory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "AssetCategory Id does not exist"));
                return bc;
            }

            assetCategory.Modified(updateCommand.ModifiedById, updateCommand.Name, updateCommand.CodeErp, updateCommand.Params);

            Persist(assetCategory, updateCommand.Id);

            var response = _autoMapper.Map<AssetCategoryModificationUiModel>(assetCategory);
            response.Message = $"AssetCategory id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(AssetCategory assetCategory, long id)
        {
            _assetCategoryRepository.Save(assetCategory, id);
            _uOf.Commit();
        }
    }
}
