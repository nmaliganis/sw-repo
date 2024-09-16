using sw.admin.contracts.ContractRepositories;
using sw.admin.model;
using sw.admin.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.admin.repository.DbContexts;

namespace sw.admin.repository.Repositories
{
    public class DepartmentRepository : RepositoryBase<Department, long>, IDepartmentRepository
    {
        public DepartmentRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<Department> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from dep in Context.Departments
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where dep.Active
                         && com.Active
                      select dep;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Department>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<Department>(result, qry.Count(), (int)pageSize);
        }

        public Department FindActiveBy(long id)
        {
            var qry = from dep in Context.Departments
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where dep.Active
                         && com.Active
                         && dep.Id == id
                      select dep;

            return qry.FirstOrDefault();
        }

        public Department FindDepartmentByName(string name)
        {
            var qry = from dep in Context.Departments
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where dep.Active
                         && com.Active
                         && dep.Name == name
                      select dep;

            return qry.FirstOrDefault();
        }
    }
}
