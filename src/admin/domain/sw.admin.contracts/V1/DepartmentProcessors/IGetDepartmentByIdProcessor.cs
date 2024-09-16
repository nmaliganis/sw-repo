using sw.admin.common.dtos.V1.Vms.Departments;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.contracts.V1.DepartmentProcessors
{
    public interface IGetDepartmentByIdProcessor
    {
        Task<BusinessResult<DepartmentUiModel>> GetDepartmentByIdAsync(long id);
    }
}
