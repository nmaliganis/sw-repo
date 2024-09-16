using sw.asset.common.dtos.Cqrs.Events;
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
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.V1.EventService;

public class GetEventHistoryByContainerIdProcessor :
    IGetEventHistoryByContainerIdProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly ISensorRepository _sensorRepository;
    public GetEventHistoryByContainerIdProcessor(IEventHistoryRepository eventHistoryRepository, IAutoMapper autoMapper, IPropertyMappingService propertyMappingService, ISensorRepository sensorRepository)
    {
        _eventHistoryRepository = eventHistoryRepository;
        _propertyMappingService = propertyMappingService;
        _autoMapper = autoMapper;
        _sensorRepository = sensorRepository;
    }

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryByContainerIdAsync(GetEventHistoryByContainerIdQuery qry, CancellationToken cancellationToken)
    {
        var sensorToBeFetched = _sensorRepository.FindByAllByContainerId(qry.ContainerId).FirstOrDefault();
        var deviceImei = sensorToBeFetched!.Device.Imei;

        var collectionBeforePaging =
            _eventHistoryRepository
                .FindAllByDeviceImeiPagedOf(deviceImei, qry.PageIndex, qry.PageSize).ApplySort(qry.OrderBy + " " + qry.SortDirection,
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

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryBetweenDatesForContainerAsync(GetEventHistoryBetweenDatesForContainerQuery qry, CancellationToken cancellationToken)
    {
        var sensorToBeFetched = _sensorRepository.FindByAllByContainerId(qry.ContainerId).FirstOrDefault();
        var deviceImei = sensorToBeFetched!.Device.Imei;


        var collectionBeforePaging =
            _eventHistoryRepository.FindEventHistoryBetweenDatesAndDeviceImeiPagedOf(deviceImei, qry.Start, qry.End, qry.PageIndex, qry.PageSize).ApplySort(qry.OrderBy + " " + qry.SortDirection,
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