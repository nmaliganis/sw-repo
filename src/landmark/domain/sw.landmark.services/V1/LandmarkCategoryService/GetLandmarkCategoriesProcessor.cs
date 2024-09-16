using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkCategoryProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkCategoryService
{
    public class GetLandmarkCategoriesProcessor :
        IGetLandmarkCategoriesProcessor,
        IRequestHandler<GetLandmarkCategoriesQuery, BusinessResult<PagedList<LandmarkCategoryUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILandmarkCategoryRepository _landmarkCategoryRepository;

        public GetLandmarkCategoriesProcessor(ILandmarkCategoryRepository landmarkCategoryRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _landmarkCategoryRepository = landmarkCategoryRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<LandmarkCategoryUiModel>>> Handle(GetLandmarkCategoriesQuery qry, CancellationToken cancellationToken)
        {
            return await GetLandmarkCategoriesAsync(qry);
        }

        public async Task<BusinessResult<PagedList<LandmarkCategoryUiModel>>> GetLandmarkCategoriesAsync(GetLandmarkCategoriesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_landmarkCategoryRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<LandmarkCategoryUiModel, LandmarkCategory>());

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

            var afterPaging = PagedList<LandmarkCategory>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<LandmarkCategoryUiModel>>(afterPaging);

            var result = new PagedList<LandmarkCategoryUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<LandmarkCategoryUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
