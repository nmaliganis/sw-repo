using sw.admin.common.dtos.V1.ResourceParameters.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.admin.common.dtos.V1.Cqrs.Persons
{
    // Queries
    public record GetPersonByIdQuery(long Id) : IRequest<BusinessResult<PersonUiModel>>;

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

    // Commands
    public record CreatePersonCommand(long CreatedById, CreatePersonResourceParameters Parameters)
        : IRequest<BusinessResult<PersonCreationUiModel>>;

    public record UpdatePersonCommand(long Id, long ModifiedById, UpdatePersonResourceParameters Parameters)
        : IRequest<BusinessResult<PersonModificationUiModel>>;

    public record DeleteSoftPersonCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<PersonDeletionUiModel>>;

    public record DeleteHardPersonCommand(long Id)
        : IRequest<BusinessResult<PersonDeletionUiModel>>;
}
