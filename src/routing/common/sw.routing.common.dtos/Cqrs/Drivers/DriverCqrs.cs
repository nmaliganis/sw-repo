using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.Vms.Drivers;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.routing.common.dtos.Cqrs.Drivers
{
    // Queries
    public record GetDriverByIdQuery(long Id) : IRequest<BusinessResult<DriverUiModel>>;

    public class GetDriversQuery : GetItinerariesResourceParameters, IRequest<BusinessResult<PagedList<DriverUiModel>>>
    {
        public GetDriversQuery(GetItinerariesResourceParameters parameters) : base()
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
