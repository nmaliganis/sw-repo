using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Devices;

internal class GetDevicesHandler :
    IRequestHandler<GetDevicesQuery, BusinessResult<PagedList<DeviceUiModel>>>
{
    private readonly IGetDevicesProcessor _processor;

    public GetDevicesHandler(IGetDevicesProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<DeviceUiModel>>> Handle(GetDevicesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetDevicesAsync(qry);
    }
}