using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Locations;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;

namespace sw.routing.services.V1.Locations;

public class GetLocationsProcessor : IGetLocationsProcessor, IRequestHandler<GetLocationsQuery, BusinessResult<PagedList<LocationUiModel>>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly ILocationRepository _locationRepository;

    public GetLocationsProcessor(ILocationRepository locationRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _locationRepository = locationRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<LocationUiModel>>> Handle(GetLocationsQuery qry, CancellationToken cancellationToken)
    {
        return await GetLocationsAsync(qry);
    }

    public async Task<BusinessResult<PagedList<LocationUiModel>>> GetLocationsAsync(GetLocationsQuery qry)
    {
        var collectionBeforePaging =
            _locationRepository.FindAllActiveLocationsPagedOf(qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                _propertyMappingService.GetPropertyMapping<LocationUiModel, LocationPoint>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<LocationPoint>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        var afterPaging = PagedList<LocationPoint>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<LocationUiModel>>(afterPaging);

        var result = new PagedList<LocationUiModel>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<LocationUiModel>>(result);

        return await Task.FromResult(bc);
    }
}