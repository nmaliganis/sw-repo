using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocodedPositionProcessors;
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

namespace sw.landmark.services.V1.GeocodedPositionService
{
    public class GetGeocodedPositionsProcessor :
        IGetGeocodedPositionsProcessor,
        IRequestHandler<GetGeocodedPositionsQuery, BusinessResult<PagedList<GeocodedPositionUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IGeocodedPositionRepository _geocodedPositionRepository;

        public GetGeocodedPositionsProcessor(IGeocodedPositionRepository geocodedPositionRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _geocodedPositionRepository = geocodedPositionRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<GeocodedPositionUiModel>>> Handle(GetGeocodedPositionsQuery qry, CancellationToken cancellationToken)
        {
            return await GetGeocodedPositionsAsync(qry);
        }

        public async Task<BusinessResult<PagedList<GeocodedPositionUiModel>>> GetGeocodedPositionsAsync(GetGeocodedPositionsQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_geocodedPositionRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<GeocodedPositionUiModel, GeocodedPosition>());

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

            var afterPaging = PagedList<GeocodedPosition>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<GeocodedPositionUiModel>>(afterPaging);

            var result = new PagedList<GeocodedPositionUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<GeocodedPositionUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
