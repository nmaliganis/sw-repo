using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.contracts.V1.DepartmentProcessors
{
    public interface IGetDepartmentsProcessor
    {
        Task<BusinessResult<PagedList<DepartmentUiModel>>> GetDepartmentsAsync(GetDepartmentsQuery qry);
    }
}
