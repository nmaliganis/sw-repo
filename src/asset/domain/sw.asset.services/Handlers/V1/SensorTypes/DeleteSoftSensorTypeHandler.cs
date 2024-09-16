using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.SensorTypes;

internal class DeleteSoftSensorTypeHandler :
    IRequestHandler<DeleteSoftSensorTypeCommand, BusinessResult<SensorTypeDeletionUiModel>>
{
    private readonly IDeleteSoftSensorTypeProcessor _processor;

    public DeleteSoftSensorTypeHandler(IDeleteSoftSensorTypeProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorTypeDeletionUiModel>> Handle(DeleteSoftSensorTypeCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteSoftSensorTypeAsync(deleteCommand);
    }
}

