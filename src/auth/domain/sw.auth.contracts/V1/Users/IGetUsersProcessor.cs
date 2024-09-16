using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.auth.contracts.V1.Users;

public interface IGetUsersProcessor
{
    Task<BusinessResult<PagedList<UserUiModel>>> GetUsersAsync(GetUsersQuery qry);
    Task<BusinessResult<PagedList<UserUiModel>>> GetUsersByCompanyAsync(long companyId, GetUsersByCompanyQuery qry);
    Task<BusinessResult<PagedList<UserUiModel>>> GetUsersByDepartmentAsync(long departmentId, GetUsersByDepartmentQuery qry);
}