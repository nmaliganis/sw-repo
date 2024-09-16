using sw.landmark.common.dtos.V1.Cqrs.Landmarks;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.LandmarkProcessors
{
    public interface IDeleteSoftLandmarkProcessor
    {
        Task<BusinessResult<LandmarkDeletionUiModel>> DeleteSoftLandmarkAsync(DeleteSoftLandmarkCommand deleteCommand);
    }
}
