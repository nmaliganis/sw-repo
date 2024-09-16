using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.common.dtos.Vms.Departments;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Departments
{
    public interface ICreateDepartmentProcessor
    {
        Task<BusinessResult<DepartmentCreationUiModel>> CreateDepartmentAsync(CreateDepartmentCommand createCommand);
    }
}
