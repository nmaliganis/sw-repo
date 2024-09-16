using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.common.dtos.Vms.Companies.Zones;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.asset.contracts.V1.CompanyProcessors.ZoneProcessors;

public interface IGetZonesProcessor
{
    Task<BusinessResult<PagedList<ZoneUiModel>>> GetZonesAsync(GetZonesByCompanyIdQuery qry);
}