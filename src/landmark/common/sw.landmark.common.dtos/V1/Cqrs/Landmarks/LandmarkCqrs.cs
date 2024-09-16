using sw.landmark.common.dtos.V1.ResourseParameters.Landmarks;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.landmark.common.dtos.V1.Cqrs.Landmarks
{
    // Queries
    public record GetLandmarkByIdQuery(long Id) : IRequest<BusinessResult<LandmarkUiModel>>;

    public class GetLandmarksQuery : GetLandmarksResourceParameters, IRequest<BusinessResult<PagedList<LandmarkUiModel>>>
    {
        public GetLandmarksQuery(GetLandmarksResourceParameters parameters) : base()
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
    public record CreateLandmarkCommand(long CreatedById, CreateLandmarkResourceParameters Parameters)
        : IRequest<BusinessResult<LandmarkCreationUiModel>>;

    public record UpdateLandmarkCommand(long Id, long ModifiedById, UpdateLandmarkResourceParameters Parameters)
        : IRequest<BusinessResult<LandmarkModificationUiModel>>;

    public record DeleteSoftLandmarkCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<LandmarkDeletionUiModel>>;

    public record DeleteHardLandmarkCommand(long Id)
        : IRequest<BusinessResult<LandmarkDeletionUiModel>>;
}
