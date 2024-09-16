using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.asset.model.Assets.Vehicles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.VehicleService
{
    public class GetVehiclesProcessor :
        IGetVehiclesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehiclesProcessor(IVehicleRepository vehicleRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<VehicleUiModel>>> GetVehiclesAsync(GetVehiclesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_vehicleRepository.FindAllActivePagedOf(qry.PageIndex, qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<VehicleUiModel, Vehicle>());

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

            var afterPaging = PagedList<Vehicle>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<VehicleUiModel>>(afterPaging);

            var result = new PagedList<VehicleUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<VehicleUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
