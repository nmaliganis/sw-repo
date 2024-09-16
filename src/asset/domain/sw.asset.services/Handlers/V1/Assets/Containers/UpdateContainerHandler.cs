using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class UpdateContainerHandler :
    IRequestHandler<UpdateContainerCommand, BusinessResult<ContainerUiModel>>
{
    private readonly IUpdateContainerProcessor _processor;

    public UpdateContainerHandler(IUpdateContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerUiModel>> Handle(UpdateContainerCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateContainerAsync(updateCommand);
    }
}