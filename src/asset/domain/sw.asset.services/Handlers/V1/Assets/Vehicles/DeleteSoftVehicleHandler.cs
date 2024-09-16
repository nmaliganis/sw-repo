using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Vehicles;

internal class DeleteSoftVehicleHandler :
    IRequestHandler<DeleteSoftVehicleCommand, BusinessResult<VehicleDeletionUiModel>>
{
    private readonly IDeleteSoftVehicleProcessor _processor;

    public DeleteSoftVehicleHandler(IDeleteSoftVehicleProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<VehicleDeletionUiModel>> Handle(DeleteSoftVehicleCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteSoftVehicleAsync(deleteCommand);
    }
}
