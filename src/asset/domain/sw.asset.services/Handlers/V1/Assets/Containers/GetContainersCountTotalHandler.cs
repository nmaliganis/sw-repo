using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Vms.Assets.Containers;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class GetContainersCountTotalHandler : IRequestHandler<GetContainersCountTotalQuery, BusinessResult<ContainerCountUiModel>>
{
    private readonly IGetContainersCountTotalProcessor _processor;
    public GetContainersCountTotalHandler(IGetContainersCountTotalProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerCountUiModel>> Handle(GetContainersCountTotalQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetContainersCountTotalAsync(qry);
    }
}