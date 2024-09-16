using sw.common.dtos.Vms.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;

namespace sw.auth.contracts.V1.Departments {
    public interface IGetDepartmentsProcessor {
        Task<BusinessResult<PagedList<DepartmentUiModel>>> GetDepartmentsAsync(GetDepartmentsQuery qry);
        Task<BusinessResult<PagedList<DepartmentUiModel>>> GetDepartmentsByUserAsync(GetDepartmentsByCompanyQuery qry);
    }
}
