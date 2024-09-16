using sw.asset.common.dtos.ResourceParameters.Sensor;
using sw.asset.common.dtos.Vms.Persons;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Persons;

// Queries
public record GetPersonByIdQuery(long Id) : IRequest<BusinessResult<PersonUiModel>>;
public record GetPersonByEmailQuery(string Email) : IRequest<BusinessResult<PersonUiModel>>;

public class GetPersonsQuery : GetPersonsResourceParameters, IRequest<BusinessResult<PagedList<PersonUiModel>>>
{
  public GetPersonsQuery(GetPersonsResourceParameters parameters) : base()
  {
    Filter = parameters.Filter;
    SearchQuery = parameters.SearchQuery;
    Fields = parameters.Fields;
    OrderBy = parameters.OrderBy;
    SortDirection = parameters.SortDirection;
    PageSize = parameters.PageSize;
    PageIndex = parameters.PageIndex;
  }
}