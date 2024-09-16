using sw.landmark.common.dtos.V1.ResourseParameters.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles
{
    // Queries
    public record GetGeocoderProfileByIdQuery(long Id) : IRequest<BusinessResult<GeocoderProfileUiModel>>;

    public class GetGeocoderProfilesQuery : GetGeocoderProfilesResourceParameters, IRequest<BusinessResult<PagedList<GeocoderProfileUiModel>>>
    {
        public GetGeocoderProfilesQuery(GetGeocoderProfilesResourceParameters parameters) : base()
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
    public record CreateGeocoderProfileCommand(long CreatedById, CreateGeocoderProfileResourceParameters Parameters)
        : IRequest<BusinessResult<GeocoderProfileCreationUiModel>>;

    public record UpdateGeocoderProfileCommand(long Id, long ModifiedById, UpdateGeocoderProfileResourceParameters Parameters)
        : IRequest<BusinessResult<GeocoderProfileModificationUiModel>>;

    public record DeleteSoftGeocoderProfileCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<GeocoderProfileDeletionUiModel>>;

    public record DeleteHardGeocoderProfileCommand(long Id)
        : IRequest<BusinessResult<GeocoderProfileDeletionUiModel>>;
}
