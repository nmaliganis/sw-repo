using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Vms.Persons;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.PersonProcessors;
using sw.asset.model.Persons;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.V1.PersonService;

public class GetPersonsProcessor :
  IGetPersonsProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IPersonRepository _personRepository;

    public GetPersonsProcessor(IPersonRepository personRepository,
      IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _personRepository = personRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<PersonUiModel>>> GetPersonsAsync(GetPersonsQuery qry)
    {
        var collectionBeforePaging =
          _personRepository.FindAllActivePagedOf(qry.PageIndex, qry.PageSize)
              .ApplySort(qry.OrderBy + " " + qry.SortDirection,
            _propertyMappingService.GetPropertyMapping<PersonUiModel, Person>());

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

        var afterPaging = PagedList<Person>
          .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<PersonUiModel>>(afterPaging);

        var result = new PagedList<PersonUiModel>(
          items,
          afterPaging.TotalCount,
          afterPaging.CurrentPage,
          afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<PersonUiModel>>(result);

        return await Task.FromResult(bc);
    }
    public async Task<BusinessResult<PagedList<PersonUiModel>>> Handle(GetPersonsQuery qry, CancellationToken cancellationToken)
    {
        return await this.GetPersonsAsync(qry);
    }
}