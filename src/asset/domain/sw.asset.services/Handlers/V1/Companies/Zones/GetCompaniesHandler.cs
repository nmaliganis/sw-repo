using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.common.dtos.Vms.Companies.Zones;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.asset.contracts.V1.CompanyProcessors.ZoneProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Companies.Zones;

internal class GetZonesHandler :
    IRequestHandler<GetZonesByCompanyIdQuery, BusinessResult<PagedList<ZoneUiModel>>>
{
    private readonly IGetZonesProcessor _processor;

    public GetZonesHandler(IGetZonesProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<ZoneUiModel>>> Handle(GetZonesByCompanyIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetZonesAsync(qry);
    }
}
