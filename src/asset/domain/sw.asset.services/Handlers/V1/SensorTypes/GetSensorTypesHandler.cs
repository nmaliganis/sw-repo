using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.SensorTypes;

internal class GetSensorTypesHandler :
    IRequestHandler<GetSensorTypesQuery, BusinessResult<PagedList<SensorTypeUiModel>>>
{
    private readonly IGetSensorTypesProcessor _processor;

    public GetSensorTypesHandler(IGetSensorTypesProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<SensorTypeUiModel>>> Handle(GetSensorTypesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetSensorTypesAsync(qry);
    }
}
