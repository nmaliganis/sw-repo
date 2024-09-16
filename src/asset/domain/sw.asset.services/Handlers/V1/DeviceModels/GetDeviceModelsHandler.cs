using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.DeviceModels;

internal class GetDeviceModelsHandler :
    IRequestHandler<GetDeviceModelsQuery, BusinessResult<PagedList<DeviceModelUiModel>>>
{
    private readonly IGetDeviceModelsProcessor _processor;

    public GetDeviceModelsHandler(IGetDeviceModelsProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<DeviceModelUiModel>>> Handle(GetDeviceModelsQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetDeviceModelsAsync(qry);
    }
}
