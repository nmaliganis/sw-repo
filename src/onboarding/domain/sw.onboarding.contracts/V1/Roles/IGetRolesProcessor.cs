using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.auth.contracts.V1.Roles
{
    public interface IGetRolesProcessor
    {
        Task<BusinessResult<PagedList<RoleUiModel>>> GetRolesAsync(GetRolesQuery qry);
    }
}
