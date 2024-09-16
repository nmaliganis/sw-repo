using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.asset.model.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceService;

public class GetDeviceProcessor :
  IGetDevicesProcessor
{
  private readonly IAutoMapper _autoMapper;
  private readonly IPropertyMappingService _propertyMappingService;
  private readonly IDeviceRepository _deviceRepository;

  public GetDeviceProcessor(IDeviceRepository deviceRepository,
    IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
  {
    _deviceRepository = deviceRepository;
    _autoMapper = autoMapper;
    _propertyMappingService = propertyMappingService;
  }

  public async Task<BusinessResult<PagedList<DeviceUiModel>>> GetDevicesAsync(GetDevicesQuery qry)
  {
    var collectionBeforePaging =
      QueryableExtensions.ApplySort(_deviceRepository.FindAllActivePagedOf(qry.PageIndex, qry.PageSize),
        qry.OrderBy + " " + qry.SortDirection,
        _propertyMappingService.GetPropertyMapping<DeviceUiModel, Device>());

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

    var afterPaging = PagedList<Device>
      .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

    var items = _autoMapper.Map<List<DeviceUiModel>>(afterPaging);

    var result = new PagedList<DeviceUiModel>(
      items,
      afterPaging.TotalCount,
      afterPaging.CurrentPage,
      afterPaging.PageSize);

    var bc = new BusinessResult<PagedList<DeviceUiModel>>(result);

    return await Task.FromResult(bc);
  }
}