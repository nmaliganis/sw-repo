using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.model.SensorTypes;

namespace sw.asset.services.V1.SensorTypeService
{
    public class GetSensorTypesProcessor :
        IGetSensorTypesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ISensorTypeRepository _sensorTypeRepository;

        public GetSensorTypesProcessor(ISensorTypeRepository sensorTypeRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _sensorTypeRepository = sensorTypeRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<SensorTypeUiModel>>> GetSensorTypesAsync(GetSensorTypesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_sensorTypeRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<SensorTypeUiModel, SensorType>());

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

            var afterPaging = PagedList<SensorType>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<SensorTypeUiModel>>(afterPaging);

            var result = new PagedList<SensorTypeUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<SensorTypeUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
