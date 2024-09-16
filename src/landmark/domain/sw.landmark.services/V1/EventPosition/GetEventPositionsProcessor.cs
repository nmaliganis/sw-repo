using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventPositionProcessors;
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

namespace sw.landmark.services.V1.EventPositionService
{
    public class GetEventPositionsProcessor :
        IGetEventPositionsProcessor,
        IRequestHandler<GetEventPositionsQuery, BusinessResult<PagedList<EventPositionUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IEventPositionRepository _eventPositionRepository;

        public GetEventPositionsProcessor(IEventPositionRepository eventPositionRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _eventPositionRepository = eventPositionRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<EventPositionUiModel>>> Handle(GetEventPositionsQuery qry, CancellationToken cancellationToken)
        {
            return await GetEventPositionsAsync(qry);
        }

        public async Task<BusinessResult<PagedList<EventPositionUiModel>>> GetEventPositionsAsync(GetEventPositionsQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_eventPositionRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<EventPositionUiModel, EventPosition>());

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

            var afterPaging = PagedList<EventPosition>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<EventPositionUiModel>>(afterPaging);

            var result = new PagedList<EventPositionUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<EventPositionUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
