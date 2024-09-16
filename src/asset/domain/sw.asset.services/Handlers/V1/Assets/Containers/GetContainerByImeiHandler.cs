using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class GetContainerByImeiHandler :
    IRequestHandler<GetContainerByImeiQuery, BusinessResult<ContainerUiModel>>
{
    private readonly IGetContainerByImeiProcessor _processor;

    public GetContainerByImeiHandler(IGetContainerByImeiProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerUiModel>> Handle(GetContainerByImeiQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetContainerByImeiAsync(qry.Imei);
    }
}
