using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Roles
{
    public interface IGetRoleByIdProcessor
    {
        Task<BusinessResult<RoleUiModel>> GetRoleByIdAsync(long id);
    }
}
