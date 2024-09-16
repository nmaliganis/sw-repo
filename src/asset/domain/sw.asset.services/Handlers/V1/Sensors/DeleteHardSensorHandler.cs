using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.V1.SensorProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Sensors;

internal class DeleteHardSensorHandler :
    IRequestHandler<DeleteHardSensorCommand, BusinessResult<SensorDeletionUiModel>>
{
    private readonly IDeleteHardSensorProcessor _processor;

    public DeleteHardSensorHandler(IDeleteHardSensorProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorDeletionUiModel>> Handle(DeleteHardSensorCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteHardSensorAsync(deleteCommand);
    }
}
