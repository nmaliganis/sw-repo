using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.EventProcessors;
using sw.asset.model.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.EventService;

public class GetEventHistoryProcessor :
    IGetEventHistoryProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    private readonly ISensorRepository _sensorRepository;
    private readonly IContainerRepository _containerRepository;

    public GetEventHistoryProcessor(IEventHistoryRepository eventHistoryRepository, ISensorRepository sensorRepository, IContainerRepository containerRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _eventHistoryRepository = eventHistoryRepository;
        _sensorRepository = sensorRepository;
        _containerRepository = containerRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryAsync(GetEventHistoryQuery qry)
    {
        var collectionBeforePaging =
            QueryableExtensions.ApplySort(_eventHistoryRepository.FindAllPagedOf(qry.PageIndex, qry.PageSize),
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

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryBetweenDatesAsync(GetEventHistoryBetweenDatesQuery qry)
    {
        var collectionBeforePaging =
    QueryableExtensions.ApplySort(_eventHistoryRepository.FindEventHistoryBetweenDatesPagedOf(qry.Start, qry.End, qry.PageIndex, qry.PageSize),
        qry.OrderBy + " " + qry.SortDirection,
        _propertyMappingService.GetPropertyMapping<EventHistoryUiModel, EventHistory>());

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

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryBetweenDatesForDeviceAsync(GetEventHistoryBetweenDatesForDeviceQuery qry)
    {
        var collectionBeforePaging =
    QueryableExtensions.ApplySort(_eventHistoryRepository.FindEventHistoryBetweenDatesAndDeviceImeiPagedOf(qry.DeviceImei, qry.Start, qry.End, qry.PageIndex, qry.PageSize),
        qry.OrderBy + " " + qry.SortDirection,
        _propertyMappingService.GetPropertyMapping<EventHistoryUiModel, EventHistory>());

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

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryByDeviceAsync(GetEventHistoryByDeviceQuery qry)
    {
        var collectionBeforePaging =
            QueryableExtensions.ApplySort(_eventHistoryRepository.FindAllByDeviceImeiPagedOf(qry.DeviceImei,qry.PageIndex, qry.PageSize),
                qry.OrderBy + " " + qry.SortDirection,
                _propertyMappingService.GetPropertyMapping<EventHistoryUiModel, EventHistory>());
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

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryNLastRecordsAsync(GetEventHistoryNLastRecordsQuery qry)
    {
        var collectionBeforePaging =
            QueryableExtensions.ApplySort(_eventHistoryRepository.FindByNLastRecordsPagedOf(qry.N, qry.PageIndex, qry.PageSize),
                qry.OrderBy + " " + qry.SortDirection,
                _propertyMappingService.GetPropertyMapping<EventHistoryUiModel, EventHistory>());
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
