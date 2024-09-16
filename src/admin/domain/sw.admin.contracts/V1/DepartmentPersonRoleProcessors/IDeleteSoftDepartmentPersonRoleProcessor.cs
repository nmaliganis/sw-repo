using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.DepartmentPersonRoleProcessors
{
    public interface IDeleteSoftDepartmentPersonRoleProcessor
    {
        Task<BusinessResult<DepartmentPersonRoleDeletionUiModel>> DeleteSoftDepartmentPersonRoleAsync(DeleteSoftDepartmentPersonRoleCommand deleteCommand);
    }
}
