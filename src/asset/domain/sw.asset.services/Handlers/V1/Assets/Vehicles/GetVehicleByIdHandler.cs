using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Vehicles;

internal class GetVehicleByIdHandler :
    IRequestHandler<GetVehicleByIdQuery, BusinessResult<VehicleUiModel>>
{
    private readonly IGetVehicleByIdProcessor _processor;

    public GetVehicleByIdHandler(IGetVehicleByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<VehicleUiModel>> Handle(GetVehicleByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetVehicleByIdAsync(qry.Id);
    }
}
