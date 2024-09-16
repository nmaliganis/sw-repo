using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.EventPositionProcessors
{
    public interface IDeleteSoftEventPositionProcessor
    {
        Task<BusinessResult<EventPositionDeletionUiModel>> DeleteSoftEventPositionAsync(DeleteSoftEventPositionCommand deleteCommand);
    }
}
