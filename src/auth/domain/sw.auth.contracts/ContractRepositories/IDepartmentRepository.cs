using sw.auth.model.Departments;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.auth.contracts.ContractRepositories {
    public interface IDepartmentRepository : IRepository<Department, long> {
        QueryResult<Department> FindAllActivePagedOf(int? pageNum, int? pageSize);
        QueryResult<Department> FindAllActiveByCompanyPagedOf(long companyId, int? pageNum, int? pageSize);
        Department FindActiveById(long id);
        Department FindActiveByName(string name);
    }
}
