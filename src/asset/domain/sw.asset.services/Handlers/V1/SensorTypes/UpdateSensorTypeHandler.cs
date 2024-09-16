using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.SensorTypes;

internal class UpdateSensorTypeHandler :
    IRequestHandler<UpdateSensorTypeCommand, BusinessResult<SensorTypeModificationUiModel>>
{
    private readonly IUpdateSensorTypeProcessor _processor;

    public UpdateSensorTypeHandler(IUpdateSensorTypeProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorTypeModificationUiModel>> Handle(UpdateSensorTypeCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateSensorTypeAsync(updateCommand);
    }
}
