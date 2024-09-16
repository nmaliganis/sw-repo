using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.CompanyProcessors;

public interface IGetCompaniesProcessor
{
    Task<BusinessResult<PagedList<CompanyUiModel>>> GetCompaniesAsync(GetCompaniesQuery qry);
}