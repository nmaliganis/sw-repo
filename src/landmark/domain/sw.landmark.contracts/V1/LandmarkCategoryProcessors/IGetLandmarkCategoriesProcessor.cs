using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.LandmarkCategoryProcessors
{
    public interface IGetLandmarkCategoriesProcessor
    {
        Task<BusinessResult<PagedList<LandmarkCategoryUiModel>>> GetLandmarkCategoriesAsync(GetLandmarkCategoriesQuery qry);
    }
}
