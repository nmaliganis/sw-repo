using sw.landmark.common.dtos.V1.ResourseParameters.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions
{
    // Queries
    public record GetGeocodedPositionByIdQuery(long Id) : IRequest<BusinessResult<GeocodedPositionUiModel>>;

    public class GetGeocodedPositionsQuery : GetGeocodedPositionsResourceParameters, IRequest<BusinessResult<PagedList<GeocodedPositionUiModel>>>
    {
        public GetGeocodedPositionsQuery(GetGeocodedPositionsResourceParameters parameters) : base()
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
    public record CreateGeocodedPositionCommand(long CreatedById, CreateGeocodedPositionResourceParameters Parameters)
        : IRequest<BusinessResult<GeocodedPositionCreationUiModel>>;

    public record UpdateGeocodedPositionCommand(long Id, long ModifiedById, UpdateGeocodedPositionResourceParameters Parameters)
        : IRequest<BusinessResult<GeocodedPositionModificationUiModel>>;

    public record DeleteSoftGeocodedPositionCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<GeocodedPositionDeletionUiModel>>;

    public record DeleteHardGeocodedPositionCommand(long Id)
        : IRequest<BusinessResult<GeocodedPositionDeletionUiModel>>;
}
