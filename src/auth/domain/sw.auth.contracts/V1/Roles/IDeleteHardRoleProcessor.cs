using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Roles;
using sw.auth.common.dtos.Vms.Roles;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Roles
{
    public interface IDeleteHardRoleProcessor
    {
        Task<BusinessResult<RoleDeletionUiModel>> DeleteHardRoleAsync(DeleteHardRoleCommand deleteCommand);
    }
}
