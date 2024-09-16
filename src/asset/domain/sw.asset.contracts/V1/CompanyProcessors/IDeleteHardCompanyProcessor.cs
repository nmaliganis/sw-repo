using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.CompanyProcessors
{
    public interface IDeleteHardCompanyProcessor
    {
        Task<BusinessResult<CompanyDeletionUiModel>> DeleteHardCompanyAsync(DeleteHardCompanyCommand deleteCommand);
    }
}
