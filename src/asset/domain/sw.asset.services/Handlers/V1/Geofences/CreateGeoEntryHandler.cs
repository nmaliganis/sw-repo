using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Geofences;

internal class CreateGeoEntryHandler :
    IRequestHandler<CreateGeoEntryCommand, BusinessResult<GeoEntryUiModel>>
{
    private readonly ICreateGeofenceProcessor _processor;

    public CreateGeoEntryHandler(ICreateGeofenceProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<GeoEntryUiModel>> Handle(CreateGeoEntryCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateGeoEntryAsync(createCommand);
    }
}
