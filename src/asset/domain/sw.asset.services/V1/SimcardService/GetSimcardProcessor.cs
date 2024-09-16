using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.asset.model.Devices.Simcards;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;

namespace sw.asset.services.V1.SimcardService;

public class GetSimcardProcessor :
  IGetSimcardsProcessor
{
  private readonly IAutoMapper _autoMapper;
  private readonly IPropertyMappingService _propertyMappingService;
  private readonly ISimcardRepository _simcardRepository;

  public GetSimcardProcessor(ISimcardRepository simcardRepository,
    IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
  {
    _simcardRepository = simcardRepository;
    _autoMapper = autoMapper;
    _propertyMappingService = propertyMappingService;
  }

  public async Task<BusinessResult<PagedList<SimcardUiModel>>> GetSimcardsAsync(GetSimcardsQuery qry)
  {
    var collectionBeforePaging =
      QueryableExtensions.ApplySort(_simcardRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
        qry.OrderBy + " " + qry.SortDirection,
        _propertyMappingService.GetPropertyMapping<SimcardUiModel, Simcard>());

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

    var afterPaging = PagedList<Simcard>
      .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

    var items = _autoMapper.Map<List<SimcardUiModel>>(afterPaging);

    var result = new PagedList<SimcardUiModel>(
      items,
      afterPaging.TotalCount,
      afterPaging.CurrentPage,
      afterPaging.PageSize);

    var bc = new BusinessResult<PagedList<SimcardUiModel>>(result);

    return await Task.FromResult(bc);
  }
}// Class  :GetSimcardProcessor