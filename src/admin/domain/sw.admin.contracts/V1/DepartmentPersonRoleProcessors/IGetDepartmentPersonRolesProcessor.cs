using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.DepartmentPersonRoleProcessors
{
    public interface IGetDepartmentPersonRolesProcessor
    {
        Task<BusinessResult<PagedList<DepartmentPersonRoleUiModel>>> GetDepartmentPersonRolesAsync(GetDepartmentPersonRolesQuery qry);
    }
}
