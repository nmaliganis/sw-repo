using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Geofences;

internal class UpdateGeofenceHandler :
    IRequestHandler<UpdateGeofenceCommand, BusinessResult<GeofenceModificationUiModel>>
{
    private readonly IUpdateGeofenceProcessor _processor;

    public UpdateGeofenceHandler(IUpdateGeofenceProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<GeofenceModificationUiModel>> Handle(UpdateGeofenceCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateGeofenceAsync(updateCommand);
    }
}