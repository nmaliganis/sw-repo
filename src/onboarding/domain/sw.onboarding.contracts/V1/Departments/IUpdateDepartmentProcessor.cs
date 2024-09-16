using sw.common.dtos.Vms.Departments;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;

namespace sw.auth.contracts.V1.DepartmentProcessors
{
    public interface IUpdateDepartmentProcessor
    {
        Task<BusinessResult<DepartmentModificationUiModel>> UpdateDepartmentAsync(UpdateDepartmentCommand updateCommand);
    }
}
