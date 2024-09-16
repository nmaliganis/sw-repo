using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class DeleteSoftContainerHandler :
    IRequestHandler<DeleteSoftContainerCommand, BusinessResult<ContainerDeletionUiModel>>
{
    private readonly IDeleteSoftContainerProcessor _processor;

    public DeleteSoftContainerHandler(IDeleteSoftContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerDeletionUiModel>> Handle(DeleteSoftContainerCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteSoftContainerAsync(deleteCommand);
    }
}
