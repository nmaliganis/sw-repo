using sw.landmark.common.dtos.V1.ResourseParameters.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.landmark.common.dtos.V1.Cqrs.EventHistories
{
    // Queries
    public record GetEventHistoryByIdQuery(long Id) : IRequest<BusinessResult<EventHistoryUiModel>>;

    public class GetEventHistoriesQuery : GetEventHistoriesResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
    {
        public GetEventHistoriesQuery(GetEventHistoriesResourceParameters parameters) : base()
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
    public record CreateEventHistoryCommand(long CreatedById, CreateEventHistoryResourceParameters Parameters)
        : IRequest<BusinessResult<EventHistoryCreationUiModel>>;

    public record UpdateEventHistoryCommand(long Id, long ModifiedById, UpdateEventHistoryResourceParameters Parameters)
        : IRequest<BusinessResult<EventHistoryModificationUiModel>>;

    public record DeleteSoftEventHistoryCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<EventHistoryDeletionUiModel>>;

    public record DeleteHardEventHistoryCommand(long Id)
        : IRequest<BusinessResult<EventHistoryDeletionUiModel>>;
}
