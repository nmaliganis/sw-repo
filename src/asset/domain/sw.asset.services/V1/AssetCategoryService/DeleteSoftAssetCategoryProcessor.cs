using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Assets.Categories;

namespace sw.asset.services.V1.AssetCategoryService
{
    public class DeleteSoftAssetCategoryProcessor :
        IDeleteSoftAssetCategoryProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IAssetCategoryRepository _assetCategoryRepository;

        public DeleteSoftAssetCategoryProcessor(IUnitOfWork uOf, IAssetCategoryRepository assetCategoryRepository)
        {
            _uOf = uOf;
            _assetCategoryRepository = assetCategoryRepository;
        }

        public async Task<BusinessResult<AssetCategoryDeletionUiModel>> DeleteSoftAssetCategoryAsync(DeleteSoftAssetCategoryCommand deleteCommand)
        {
            var bc = new BusinessResult<AssetCategoryDeletionUiModel>(new AssetCategoryDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var assetCategory = _assetCategoryRepository.FindBy(deleteCommand.Id);
            if (assetCategory is null || !assetCategory.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "AssetCategory Id does not exist"));
                return bc;
            }

            assetCategory.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(assetCategory, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"AssetCategory with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(AssetCategory assetCategory, long id)
        {
            _assetCategoryRepository.Save(assetCategory, id);
            _uOf.Commit();
        }
    }
}
