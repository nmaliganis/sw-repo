using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class SearchContainersBetweenLevelsHandler : 
    IRequestHandler<GetContainersByVolumeInZonesQuery, BusinessResult<List<ContainerUiModel>>>
{
    private readonly ISearchContainersBetweenLevelsProcessor _processor;
    public SearchContainersBetweenLevelsHandler(ISearchContainersBetweenLevelsProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<List<ContainerUiModel>>> Handle(GetContainersByVolumeInZonesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.SearchContainersBetweenLevelsAsync(qry);
    }
}