using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.V1.SensorProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Sensors;

internal class GetSensorByIdHandler :
    IRequestHandler<GetSensorByIdQuery, BusinessResult<SensorUiModel>>
{
    private readonly IGetSensorByIdProcessor _processor;

    public GetSensorByIdHandler(IGetSensorByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SensorUiModel>> Handle(GetSensorByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetSensorByIdAsync(qry.Id);
    }
}
