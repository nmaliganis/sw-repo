using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class DeleteHardContainerHandler :
    IRequestHandler<DeleteHardContainerCommand, BusinessResult<ContainerDeletionUiModel>>
{
    private readonly IDeleteHardContainerProcessor _processor;

    public DeleteHardContainerHandler(IDeleteHardContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerDeletionUiModel>> Handle(DeleteHardContainerCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteHardContainerAsync(deleteCommand);
    }
}
