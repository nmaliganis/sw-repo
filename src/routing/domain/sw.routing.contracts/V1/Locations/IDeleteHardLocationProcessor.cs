using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.infrastructure.BrokenRules;

namespace sw.routing.contracts.V1.Locations
{
    public interface IDeleteHardLocationProcessor
    {
        Task<BusinessResult<LocationDeletionUiModel>> DeleteHardLocationAsync(DeleteHardLocationCommand deleteCommand);
    }
}
