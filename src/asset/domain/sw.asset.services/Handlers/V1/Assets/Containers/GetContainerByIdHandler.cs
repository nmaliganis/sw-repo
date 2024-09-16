using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class GetContainerByIdHandler :
    IRequestHandler<GetContainerByIdQuery, BusinessResult<ContainerUiModel>>
{
    private readonly IGetContainerByIdProcessor _processor;

    public GetContainerByIdHandler(IGetContainerByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerUiModel>> Handle(GetContainerByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetContainerByIdAsync(qry.Id);
    }
}
