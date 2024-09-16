using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Companies;
using sw.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Companies
{
    public interface IGetCompanyByIdProcessor
    {
        Task<BusinessResult<CompanyUiModel>> GetCompanyByIdAsync(long id);
    }
}
