using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Geofences;

internal class GetGeoEntryByKeyHandler :
    IRequestHandler<GetGeoEntryByKeyQuery, BusinessResult<GeoEntryUiModel>>
{
    private readonly IGetGeoEntryByKeyProcessor _processor;

    public GetGeoEntryByKeyHandler(IGetGeoEntryByKeyProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<GeoEntryUiModel>> Handle(GetGeoEntryByKeyQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetGeoEntryByKeyAsync(qry.Key);
    }
}
