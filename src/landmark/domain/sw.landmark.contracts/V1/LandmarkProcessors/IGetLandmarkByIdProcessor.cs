using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.LandmarkProcessors
{
    public interface IGetLandmarkByIdProcessor
    {
        Task<BusinessResult<LandmarkUiModel>> GetLandmarkByIdAsync(long id);
    }
}
