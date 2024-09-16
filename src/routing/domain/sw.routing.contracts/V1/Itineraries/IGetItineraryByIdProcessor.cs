using System.Threading.Tasks;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.infrastructure.BrokenRules;

namespace sw.routing.contracts.V1.Itineraries;

public interface IGetItineraryByIdProcessor
{
    Task<BusinessResult<ItineraryUiModel>> GetItineraryByIdAsync(long id);
}