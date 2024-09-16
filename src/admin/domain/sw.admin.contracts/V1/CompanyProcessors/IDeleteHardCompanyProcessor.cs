using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.CompanyProcessors
{
    public interface IDeleteHardCompanyProcessor
    {
        Task<BusinessResult<CompanyDeletionUiModel>> DeleteHardCompanyAsync(DeleteHardCompanyCommand deleteCommand);
    }
}
