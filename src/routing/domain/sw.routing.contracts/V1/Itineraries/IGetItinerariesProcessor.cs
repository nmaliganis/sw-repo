using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.routing.contracts.V1.Itineraries;

public interface IGetItinerariesProcessor
{
    Task<BusinessResult<PagedList<ItineraryUiModel>>> GetItinerariesAsync(GetItinerariesQuery qry);
}