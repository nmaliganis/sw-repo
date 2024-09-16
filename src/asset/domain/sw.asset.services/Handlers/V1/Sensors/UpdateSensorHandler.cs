using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.V1.SensorProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Sensors;

internal class UpdateSensorHandler :
    IRequestHandler<UpdateSensorCommand, BusinessResult<SensorModificationUiModel>>
{
    private readonly IUpdateSensorProcessor _processor;

    public UpdateSensorHandler(IUpdateSensorProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorModificationUiModel>> Handle(UpdateSensorCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateSensorAsync(updateCommand);
    }
}