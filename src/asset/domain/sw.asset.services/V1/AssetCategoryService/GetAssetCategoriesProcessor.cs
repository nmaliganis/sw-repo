using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.model.Assets.Categories;

namespace sw.asset.services.V1.AssetCategoryService
{
    public class GetAssetCategoriesProcessor :
        IGetAssetCategoriesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IAssetCategoryRepository _assetCategoryRepository;

        public GetAssetCategoriesProcessor(IAssetCategoryRepository assetCategoryRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _assetCategoryRepository = assetCategoryRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<AssetCategoryUiModel>>> GetAssetCategoriesAsync(GetAssetCategoriesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_assetCategoryRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<AssetCategoryUiModel, AssetCategory>());

            if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = qry.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = collectionBeforePaging
                    .QueriedItems
                    .AsEnumerable()
                    .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery)
                    .AsQueryable();
            }

            var afterPaging = PagedList<AssetCategory>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<AssetCategoryUiModel>>(afterPaging);

            var result = new PagedList<AssetCategoryUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<AssetCategoryUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
