using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Devices;

internal class GetDeviceByIdHandler :
    IRequestHandler<GetDeviceByIdQuery, BusinessResult<DeviceUiModel>>
{
    private readonly IGetDeviceByIdProcessor _processor;

    public GetDeviceByIdHandler(IGetDeviceByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<DeviceUiModel>> Handle(GetDeviceByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetDeviceByIdAsync(qry.Id);
    }
}
