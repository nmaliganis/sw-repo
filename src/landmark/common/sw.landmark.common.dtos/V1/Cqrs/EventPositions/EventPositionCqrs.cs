using sw.landmark.common.dtos.V1.ResourseParameters.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.landmark.common.dtos.V1.Cqrs.EventPositions
{
    // Queries
    public record GetEventPositionByIdQuery(long Id) : IRequest<BusinessResult<EventPositionUiModel>>;

    public class GetEventPositionsQuery : GetEventPositionsResourceParameters, IRequest<BusinessResult<PagedList<EventPositionUiModel>>>
    {
        public GetEventPositionsQuery(GetEventPositionsResourceParameters parameters) : base()
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
    public record CreateEventPositionCommand(long CreatedById, CreateEventPositionResourceParameters Parameters)
        : IRequest<BusinessResult<EventPositionCreationUiModel>>;

    public record UpdateEventPositionCommand(long Id, long ModifiedById, UpdateEventPositionResourceParameters Parameters)
        : IRequest<BusinessResult<EventPositionModificationUiModel>>;

    public record DeleteSoftEventPositionCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<EventPositionDeletionUiModel>>;

    public record DeleteHardEventPositionCommand(long Id)
        : IRequest<BusinessResult<EventPositionDeletionUiModel>>;
}
