using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.Vms.Vehicles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.routing.common.dtos.Cqrs.Vehicles
{
    // Queries
    public record GetVehicleByIdQuery(long Id) : IRequest<BusinessResult<VehicleUiModel>>;

    public class GetVehiclesQuery : GetItinerariesResourceParameters, IRequest<BusinessResult<PagedList<VehicleUiModel>>>
    {
        public GetVehiclesQuery(GetItinerariesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }
}
