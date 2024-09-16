using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.V1.SensorProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Sensors;

internal class GetSensorsHandler :
    IRequestHandler<GetSensorsQuery, BusinessResult<PagedList<SensorUiModel>>>
{
    private readonly IGetSensorsProcessor _processor;

    public GetSensorsHandler(IGetSensorsProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<SensorUiModel>>> Handle(GetSensorsQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetSensorsAsync(qry);
    }
}
