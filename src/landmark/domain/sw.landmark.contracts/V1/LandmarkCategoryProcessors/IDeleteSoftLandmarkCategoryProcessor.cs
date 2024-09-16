using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.LandmarkCategoryProcessors
{
    public interface IDeleteSoftLandmarkCategoryProcessor
    {
        Task<BusinessResult<LandmarkCategoryDeletionUiModel>> DeleteSoftLandmarkCategoryAsync(DeleteSoftLandmarkCategoryCommand deleteCommand);
    }
}
