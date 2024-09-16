using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.CompanyProcessors
{
    public interface IDeleteSoftCompanyProcessor
    {
        Task<BusinessResult<CompanyDeletionUiModel>> DeleteSoftCompanyAsync(DeleteSoftCompanyCommand deleteCommand);
    }
}
