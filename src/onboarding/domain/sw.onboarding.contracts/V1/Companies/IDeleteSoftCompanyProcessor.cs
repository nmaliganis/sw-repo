using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Companies
{
    public interface IDeleteSoftCompanyProcessor
    {
        Task<BusinessResult<CompanyDeletionUiModel>> DeleteSoftCompanyAsync(DeleteSoftCompanyCommand deleteCommand);
    }
}
