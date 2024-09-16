using sw.landmark.common.dtos.V1.ResourseParameters.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories
{
    // Queries
    public record GetLandmarkCategoryByIdQuery(long Id) : IRequest<BusinessResult<LandmarkCategoryUiModel>>;

    public class GetLandmarkCategoriesQuery : GetLandmarkCategoriesResourceParameters, IRequest<BusinessResult<PagedList<LandmarkCategoryUiModel>>>
    {
        public GetLandmarkCategoriesQuery(GetLandmarkCategoriesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            SortDirection = parameters.SortDirection;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateLandmarkCategoryCommand(long CreatedById, CreateLandmarkCategoryResourceParameters Parameters)
        : IRequest<BusinessResult<LandmarkCategoryCreationUiModel>>;

    public record UpdateLandmarkCategoryCommand(long Id, long ModifiedById, UpdateLandmarkCategoryResourceParameters Parameters)
        : IRequest<BusinessResult<LandmarkCategoryModificationUiModel>>;

    public record DeleteSoftLandmarkCategoryCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<LandmarkCategoryDeletionUiModel>>;

    public record DeleteHardLandmarkCategoryCommand(long Id)
        : IRequest<BusinessResult<LandmarkCategoryDeletionUiModel>>;
}
