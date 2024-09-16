using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.CompanyProcessors
{
    public interface IGetCompaniesProcessor
    {
        Task<BusinessResult<PagedList<CompanyUiModel>>> GetCompaniesAsync(GetCompaniesQuery qry);
    }
}
