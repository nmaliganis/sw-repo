using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.V1.SensorProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Sensors;

internal class CreateSensorHandler :
    IRequestHandler<CreateSensorCommand, BusinessResult<SensorUiModel>>
{
    private readonly ICreateSensorProcessor _processor;

    public CreateSensorHandler(ICreateSensorProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorUiModel>> Handle(CreateSensorCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateSensorAsync(createCommand);
    }
}
