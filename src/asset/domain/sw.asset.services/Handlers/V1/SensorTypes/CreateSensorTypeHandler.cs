using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.SensorTypes;

internal class CreateSensorTypeHandler :
    IRequestHandler<CreateSensorTypeCommand, BusinessResult<SensorTypeUiModel>>
{
    private readonly ICreateSensorTypeProcessor _processor;

    public CreateSensorTypeHandler(ICreateSensorTypeProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorTypeUiModel>> Handle(CreateSensorTypeCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateSensorTypeAsync(createCommand);
    }
}