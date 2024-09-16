using sw.asset.common.dtos.ResourceParameters.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.model.Geofence;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Geofences;

// Queries
public record GetGeofenceByKeyQuery(string Key) : IRequest<BusinessResult<LandmarkUiModel>>;

public record GetGeoEntryByKeyQuery(string Key) : IRequest<BusinessResult<GeoEntryUiModel>>;

public class GetGeofencesQuery : GetGeofenceResourceParameters, IRequest<BusinessResult<PagedList<GeofenceUiModel>>>
{
    public GetGeofencesQuery(GetGeofenceResourceParameters parameters) : base()
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
public record CreateGeofenceCommand(long CreatedById, CreateGeofenceResourceParameters Parameters)
    : IRequest<BusinessResult<GeofenceUiModel>>;

public record CreateMunicipalityCommand(long CreatedById, long MunicipalityId, string MunicipalityName)
    : IRequest<BusinessResult<MunicipalityUiModel>>;

public record CreateGeoEntryCommand(long CreatedById, string PhysAddress, CreateGeoEntryResourceParameters Parameters)
    : IRequest<BusinessResult<GeoEntryUiModel>>;

public record UpdateGeofenceCommand(string Key, long ModifiedById, UpdateGeofenceResourceParameters Parameters)
: IRequest<BusinessResult<GeofenceModificationUiModel>>;


