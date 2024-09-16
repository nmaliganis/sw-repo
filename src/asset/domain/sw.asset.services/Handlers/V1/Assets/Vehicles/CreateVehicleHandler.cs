using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Vehicles;

internal class CreateVehicleHandler :
    IRequestHandler<CreateVehicleCommand, BusinessResult<VehicleCreationUiModel>>
{
    private readonly ICreateVehicleProcessor _processor;

    public CreateVehicleHandler(ICreateVehicleProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<VehicleCreationUiModel>> Handle(CreateVehicleCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateVehicleAsync(createCommand);
    }
}
