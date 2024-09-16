using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class GetContainersHandler :
    IRequestHandler<GetContainersQuery, BusinessResult<PagedList<ContainerUiModel>>>
{
    private readonly IGetContainersProcessor _processor;

    public GetContainersHandler(IGetContainersProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<ContainerUiModel>>> Handle(GetContainersQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetContainersAsync(qry);
    }

}


internal class GetContainersByZoneIdHandler :
    IRequestHandler<GetContainersByZoneIdQuery, BusinessResult<PagedList<ContainerUiModel>>>
{
    private readonly IGetContainersProcessor _processor;

    public GetContainersByZoneIdHandler(IGetContainersProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<ContainerUiModel>>> Handle(GetContainersByZoneIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetContainersByZoneIdAsync(qry.ZoneId, qry);
    }
}