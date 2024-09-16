using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.SensorTypes;

internal class GetSensorTypeByIdHandler :
    IRequestHandler<GetSensorTypeByIdQuery, BusinessResult<SensorTypeUiModel>>
{
    private readonly IGetSensorTypeByIdProcessor _processor;

    public GetSensorTypeByIdHandler(IGetSensorTypeByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorTypeUiModel>> Handle(GetSensorTypeByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetSensorTypeByIdAsync(qry.Id);
    }
}
