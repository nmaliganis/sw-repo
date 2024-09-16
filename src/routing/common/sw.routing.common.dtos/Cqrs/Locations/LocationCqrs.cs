using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.Vms.Locations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.routing.common.dtos.Cqrs.Locations
{
    // Queries
    public record GetLocationByIdQuery(long Id) : IRequest<BusinessResult<LocationUiModel>>;

    public class GetLocationsQuery : GetItinerariesResourceParameters, IRequest<BusinessResult<PagedList<LocationUiModel>>>
    {
        public GetLocationsQuery(GetItinerariesResourceParameters parameters) : base()
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
    public record CreateLocationCommand(long CreatedById, string Name)
        : IRequest<BusinessResult<LocationUiModel>>;

    public record UpdateLocationCommand(long ModifiedById, long Id, string Name)
        : IRequest<BusinessResult<LocationModificationUiModel>>;

    public record DeleteSoftLocationCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<LocationDeletionUiModel>>;

    public record DeleteHardLocationCommand(long Id)
        : IRequest<BusinessResult<LocationDeletionUiModel>>;
}
