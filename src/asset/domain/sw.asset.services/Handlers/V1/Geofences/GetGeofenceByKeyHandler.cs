using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.asset.model.Geofence;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Geofences;

internal class GetGeofenceByKeyHandler :
    IRequestHandler<GetGeofenceByKeyQuery, BusinessResult<LandmarkUiModel>>
{
    private readonly IGetGeofenceByKeyProcessor _processor;

    public GetGeofenceByKeyHandler(IGetGeofenceByKeyProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<LandmarkUiModel>> Handle(GetGeofenceByKeyQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetGeofenceByKeyAsync(qry.Key);
    }
}
