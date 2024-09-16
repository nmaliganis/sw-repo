using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventHistoryProcessors;
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

namespace sw.landmark.services.V1.EventHistoryService
{
    public class GetEventHistoriesProcessor :
        IGetEventHistoriesProcessor,
        IRequestHandler<GetEventHistoriesQuery, BusinessResult<PagedList<EventHistoryUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IEventHistoryRepository _eventHistoryRepository;

        public GetEventHistoriesProcessor(IEventHistoryRepository eventHistoryRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _eventHistoryRepository = eventHistoryRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> Handle(GetEventHistoriesQuery qry, CancellationToken cancellationToken)
        {
            return await GetEventHistoriesAsync(qry);
        }

        public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoriesAsync(GetEventHistoriesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_eventHistoryRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<EventHistoryUiModel, EventHistory>());

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

            var afterPaging = PagedList<EventHistory>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<EventHistoryUiModel>>(afterPaging);

            var result = new PagedList<EventHistoryUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<EventHistoryUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
