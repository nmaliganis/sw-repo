using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class UpdateContainerWithLatLonByDeviceHandler :
  IRequestHandler<UpdateContainerWithLatLonByDeviceCommand, BusinessResult<ContainerModificationUiModel>>
{
    private readonly IUpdateContainerProcessor _processor;

    public UpdateContainerWithLatLonByDeviceHandler(IUpdateContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerModificationUiModel>> Handle(UpdateContainerWithLatLonByDeviceCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateContainerWithLatLotAsync(updateCommand);
    }
}


internal class UpdateContainerByNameWithLatLonHandler :
    IRequestHandler<UpdateContainerByNameWithLatLonCommand, BusinessResult<ContainerModificationUiModel>>
{
    private readonly IUpdateContainerProcessor _processor;

    public UpdateContainerByNameWithLatLonHandler(IUpdateContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerModificationUiModel>> Handle(UpdateContainerByNameWithLatLonCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateContainerWithLatLotAsync(updateCommand);
    }
}