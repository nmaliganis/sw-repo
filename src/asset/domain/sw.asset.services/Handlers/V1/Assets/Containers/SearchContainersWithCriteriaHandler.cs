using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class SearchContainersWithCriteriaHandler :
    IRequestHandler<GetContainersByCriteriaInZonesQuery, BusinessResult<List<ContainerUiModel>>>
{
    private readonly ISearchContainersWithCriteriaProcessor _processor;
    public SearchContainersWithCriteriaHandler(ISearchContainersWithCriteriaProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<List<ContainerUiModel>>> Handle(GetContainersByCriteriaInZonesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.SearchContainersWithCriteriaAsync(qry);
    }
}