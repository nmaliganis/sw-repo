using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.auth.contracts.V1.Companies
{
    public interface IGetCompaniesProcessor
    {
        Task<BusinessResult<PagedList<CompanyUiModel>>> GetCompaniesAsync(GetCompaniesQuery qry);
    }
}
