using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Geofences;

internal class CreateMunicipalityHandler :
    IRequestHandler<CreateMunicipalityCommand, BusinessResult<MunicipalityUiModel>>
{
    private readonly ICreateGeofenceProcessor _processor;

    public CreateMunicipalityHandler(ICreateGeofenceProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<MunicipalityUiModel>> Handle(CreateMunicipalityCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateMunicipalityAsync(createCommand);
    }
}
