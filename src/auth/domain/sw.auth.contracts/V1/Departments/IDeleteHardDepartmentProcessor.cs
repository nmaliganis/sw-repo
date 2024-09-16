using sw.common.dtos.Vms.Departments;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.common.dtos.Vms.Departments;

namespace sw.auth.contracts.V1.DepartmentProcessors
{
    public interface IDeleteHardDepartmentProcessor
    {
        Task<BusinessResult<DepartmentDeletionUiModel>> DeleteHardDepartmentAsync(DeleteHardDepartmentCommand deleteCommand);
    }
}
