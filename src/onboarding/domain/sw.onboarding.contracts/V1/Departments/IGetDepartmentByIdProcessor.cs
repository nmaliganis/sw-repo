using sw.common.dtos.Vms.Departments;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.auth.contracts.V1.DepartmentProcessors
{
    public interface IGetDepartmentByIdProcessor
    {
        Task<BusinessResult<DepartmentUiModel>> GetDepartmentByIdAsync(long id);
    }
}
