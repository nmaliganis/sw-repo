using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Vehicles;

internal class GetVehiclesHandler :
    IRequestHandler<GetVehiclesQuery, BusinessResult<PagedList<VehicleUiModel>>>
{
    private readonly IGetVehiclesProcessor _processor;

    public GetVehiclesHandler(IGetVehiclesProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<VehicleUiModel>>> Handle(GetVehiclesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetVehiclesAsync(qry);
    }
}
