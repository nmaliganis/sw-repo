using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.services.V1.AssetServices.ContainerService;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class GetContainersCountTotalInZonesHandler :
    IRequestHandler<GetContainersCountTotalInZonesQuery, BusinessResult<ContainerCountUiModel>>
{
    private readonly IGetContainersCountTotalInZoneProcessor _processor;
    public GetContainersCountTotalInZonesHandler(IGetContainersCountTotalInZoneProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerCountUiModel>> Handle(GetContainersCountTotalInZonesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetContainersCountTotalInZoneAsync(qry);
    }
}