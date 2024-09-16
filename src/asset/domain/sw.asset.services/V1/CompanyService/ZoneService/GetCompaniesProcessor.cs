using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.common.dtos.Vms.Companies.Zones;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.asset.contracts.V1.CompanyProcessors.ZoneProcessors;
using sw.asset.model.Companies;
using sw.asset.model.Companies.Zones;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;

namespace sw.asset.services.V1.CompanyService.ZoneService;

public class GetZonesProcessor : IGetZonesProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IZoneRepository _zoneRepository;

    public GetZonesProcessor(IZoneRepository zoneRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _zoneRepository = zoneRepository;
        _propertyMappingService = propertyMappingService;
        _autoMapper = autoMapper;
    }


    public async Task<BusinessResult<PagedList<ZoneUiModel>>> GetZonesAsync(GetZonesByCompanyIdQuery qry)
    {
        var collectionBeforePaging =
            _zoneRepository.FindAllActivePagedOf(qry.CompanyId, qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                _propertyMappingService.GetPropertyMapping<CompanyUiModel, Company>());

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

        var afterPaging = PagedList<Zone>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<ZoneUiModel>>(afterPaging);

        var result = new PagedList<ZoneUiModel>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<ZoneUiModel>>(result);

        return await Task.FromResult(bc);
    }
}