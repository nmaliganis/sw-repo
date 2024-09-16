using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.model.Sensors;

namespace sw.asset.services.V1.SensorService
{
    public class GetSensorsProcessor :
        IGetSensorsProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ISensorRepository _sensorRepository;

        public GetSensorsProcessor(ISensorRepository sensorRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _sensorRepository = sensorRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<SensorUiModel>>> GetSensorsAsync(GetSensorsQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_sensorRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<SensorUiModel, Sensor>());

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

            var afterPaging = PagedList<Sensor>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<SensorUiModel>>(afterPaging);

            var result = new PagedList<SensorUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<SensorUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
