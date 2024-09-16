using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.routing.common.dtos.Cqrs.Itineraries
{
    // Queries
    public record GetItineraryByIdQuery(long Id) : IRequest<BusinessResult<ItineraryUiModel>>;

    public class GetItinerariesQuery : GetItinerariesResourceParameters, IRequest<BusinessResult<PagedList<ItineraryUiModel>>>
    {
        public GetItinerariesQuery(GetItinerariesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateItineraryCommand(long CreatedById, CreateItineraryResourceParameters Parameters)
        : IRequest<BusinessResult<ItineraryUiModel>>;

    public record UpdateItineraryCommand(long ModifiedById, long Id, string Name)
        : IRequest<BusinessResult<ItineraryModificationUiModel>>;

    public record DeleteSoftItineraryCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<ItineraryDeletionUiModel>>;

    public record DeleteHardItineraryCommand(long Id)
        : IRequest<BusinessResult<ItineraryDeletionUiModel>>;
}
