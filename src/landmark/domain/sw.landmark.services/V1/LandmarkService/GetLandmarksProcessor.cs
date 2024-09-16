using sw.landmark.common.dtos.V1.Cqrs.Landmarks;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkProcessors;
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

namespace sw.landmark.services.V1.LandmarkService
{
    public class GetLandmarksProcessor :
        IGetLandmarksProcessor,
        IRequestHandler<GetLandmarksQuery, BusinessResult<PagedList<LandmarkUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILandmarkRepository _landmarkRepository;

        public GetLandmarksProcessor(ILandmarkRepository landmarkRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _landmarkRepository = landmarkRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<LandmarkUiModel>>> Handle(GetLandmarksQuery qry, CancellationToken cancellationToken)
        {
            return await GetLandmarksAsync(qry);
        }

        public async Task<BusinessResult<PagedList<LandmarkUiModel>>> GetLandmarksAsync(GetLandmarksQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_landmarkRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<LandmarkUiModel, Landmark>());

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

            var afterPaging = PagedList<Landmark>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<LandmarkUiModel>>(afterPaging);

            var result = new PagedList<LandmarkUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<LandmarkUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
