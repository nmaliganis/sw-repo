using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class UpdateContainerWithLatLonHandler :
  IRequestHandler<UpdateContainerWithLatLonCommand, BusinessResult<ContainerModificationUiModel>>
{
    private readonly IUpdateContainerProcessor _processor;

    public UpdateContainerWithLatLonHandler(IUpdateContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerModificationUiModel>> Handle(UpdateContainerWithLatLonCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateContainerWithLatLotAsync(updateCommand);
    }
}