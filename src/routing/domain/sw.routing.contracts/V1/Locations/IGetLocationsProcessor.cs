using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.routing.contracts.V1.Locations;

public interface IGetLocationsProcessor
{
    Task<BusinessResult<PagedList<LocationUiModel>>> GetLocationsAsync(GetLocationsQuery qry);
}