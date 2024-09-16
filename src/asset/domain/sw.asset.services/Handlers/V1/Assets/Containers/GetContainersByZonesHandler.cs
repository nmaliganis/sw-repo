using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class GetContainersByZonesHandler :
    IRequestHandler<GetContainersByZonesQuery, BusinessResult<List<ContainerUiModel>>>
{
    private readonly IGetContainersProcessor _processor;

    public GetContainersByZonesHandler(IGetContainersProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<List<ContainerUiModel>>> Handle(GetContainersByZonesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetContainersByZonesAsync(qry.Zones);
    }
}