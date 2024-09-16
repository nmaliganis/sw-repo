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
    public class CreateAssetCategoryProcessor :
        ICreateAssetCategoryProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateAssetCategoryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IAssetCategoryRepository assetCategoryRepository)
        {
            this._uOf = uOf;
            this._autoMapper = autoMapper;
            this._assetCategoryRepository = assetCategoryRepository;
        }

        public async Task<BusinessResult<AssetCategoryCreationUiModel>> CreateAssetCategoryAsync(CreateAssetCategoryCommand createCommand)
        {
            var bc = new BusinessResult<AssetCategoryCreationUiModel>(new AssetCategoryCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _assetCategoryRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "AssetCategory name already exists"));
            //}

            var assetCategory = new AssetCategory()
            {
                Name = createCommand.Name,
                CodeErp = createCommand.CodeErp,
                //Params = JsonDocument.Parse(createCommand.Params)
            };
            assetCategory.Created(createCommand.CreatedById);

            Persist(assetCategory);

            var response = _autoMapper.Map<AssetCategoryCreationUiModel>(assetCategory);
            response.Message = "AssetCategory created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(AssetCategory assetCategory)
        {
            _assetCategoryRepository.Add(assetCategory);
            _uOf.Commit();
        }
    }
}
