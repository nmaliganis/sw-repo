using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.infrastructure.BrokenRules;

namespace sw.routing.contracts.V1.Itineraries;

public interface IDeleteSoftItineraryProcessor
{
    Task<BusinessResult<ItineraryDeletionUiModel>> DeleteSoftItineraryAsync(DeleteSoftItineraryCommand deleteCommand);
}