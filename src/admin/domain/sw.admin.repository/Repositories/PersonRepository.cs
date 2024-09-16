using sw.admin.contracts.ContractRepositories;
using sw.admin.model;
using sw.admin.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.admin.repository.DbContexts;

namespace sw.admin.repository.Repositories
{
    public class PersonRepository : RepositoryBase<Person, long>, IPersonRepository
    {
        public PersonRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<Person> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from per in Context.Persons
                      join dpr in Context.DepartmentPersonRoles on per.PersonRoleId equals dpr.Id
                      join dep in Context.Departments on dpr.DepartmentId equals dep.Id
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where per.Active
                         && dpr.Active
                         && dep.Active
                         && com.Active
                      select per;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Person>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<Person>(result, qry.Count(), (int)pageSize);
        }

        public Person FindActiveBy(long id)
        {
            var qry = from per in Context.Persons
                      join dpr in Context.DepartmentPersonRoles on per.PersonRoleId equals dpr.Id
                      join dep in Context.Departments on dpr.DepartmentId equals dep.Id
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where per.Active
                         && dpr.Active
                         && dep.Active
                         && com.Active
                         && per.Id == id
                      select per;

            return qry.FirstOrDefault();
        }

        public Person FindPersonByEmail(string email)
        {
            var qry = from per in Context.Persons
                      join dpr in Context.DepartmentPersonRoles on per.PersonRoleId equals dpr.Id
                      join dep in Context.Departments on dpr.DepartmentId equals dep.Id
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where per.Active
                         && dpr.Active
                         && dep.Active
                         && com.Active
                         && per.Email == email
                      select per;

            return qry.FirstOrDefault();
        }
    }
}
