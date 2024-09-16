using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.LandmarkCategoryProcessors
{
    public interface IGetLandmarkCategoryByIdProcessor
    {
        Task<BusinessResult<LandmarkCategoryUiModel>> GetLandmarkCategoryByIdAsync(long id);
    }
}
