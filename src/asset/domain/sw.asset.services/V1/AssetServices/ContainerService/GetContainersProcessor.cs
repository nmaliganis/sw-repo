using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.asset.model.Assets.Containers;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.ContainerService;

public class GetContainersProcessor : IGetContainersProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IContainerRepository _containerRepository;

    public GetContainersProcessor(IContainerRepository containerRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _containerRepository = containerRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<ContainerUiModel>>> GetContainersAsync(GetContainersQuery qry)
    {
        var collectionBeforePaging =
            _containerRepository.FindAllActivePagedOf(qry.PageIndex, qry.PageSize).ApplySort(qry.OrderBy + " " + qry.SortDirection,
                _propertyMappingService.GetPropertyMapping<ContainerUiModel, Container>());

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

        var afterPaging = PagedList<Container>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<ContainerUiModel>>(afterPaging);

        var result = new PagedList<ContainerUiModel>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<ContainerUiModel>>(result);

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<PagedList<ContainerUiModel>>> GetContainersByZoneIdAsync(long zoneId, GetContainersByZoneIdQuery qry)
    {
        var collectionBeforePaging =
            _containerRepository.FindAllActiveByZoneIdPagedOf(zoneId, qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<ContainerUiModel, Container>());

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

        var afterPaging = PagedList<Container>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<ContainerUiModel>>(afterPaging);

        var result = new PagedList<ContainerUiModel>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<ContainerUiModel>>(result);

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<List<ContainerUiModel>>> GetContainersByZonesAsync(List<long> zones)
    {
	    var containers =
		    await Task.Run(() => _containerRepository.FindAllActiveByZones(zones));

		var bc = new BusinessResult<List<ContainerUiModel>>(_autoMapper.Map<List<ContainerUiModel>>(containers));

	    return await Task.FromResult(bc);
	}
}//Class : GetContainersProcessor