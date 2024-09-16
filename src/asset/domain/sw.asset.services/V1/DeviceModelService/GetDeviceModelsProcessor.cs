using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.asset.model.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceModelService
{
    public class GetDeviceModelsProcessor :
        IGetDeviceModelsProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IDeviceModelRepository _deviceModelRepository;

        public GetDeviceModelsProcessor(IDeviceModelRepository deviceModelRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _deviceModelRepository = deviceModelRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<DeviceModelUiModel>>> GetDeviceModelsAsync(GetDeviceModelsQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_deviceModelRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<DeviceModelUiModel, DeviceModel>());

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

            var afterPaging = PagedList<DeviceModel>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<DeviceModelUiModel>>(afterPaging);

            var result = new PagedList<DeviceModelUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<DeviceModelUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
