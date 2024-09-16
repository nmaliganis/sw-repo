using sw.asset.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;
using sw.asset.common.dtos.Vms.Companies.Zones;

namespace sw.asset.contracts.V1.CompanyProcessors;

public interface IGetCompanyByIdProcessor
{
    Task<BusinessResult<CompanyUiModel>> GetCompanyByIdAsync(long id);
}