using sw.auth.contracts.ContractRepositories;
using sw.auth.model.Companies;
using sw.auth.model.Departments;
using sw.auth.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using System;
using System.Linq;

namespace sw.auth.repository.Repositories {
    public class DepartmentRepository : RepositoryBase<Department, long>, IDepartmentRepository {
        public DepartmentRepository(ISession session)
            : base(session) {
        }

        public QueryResult<Department> FindAllActivePagedOf(int? pageNum, int? pageSize) {
            var query = this.Session.QueryOver<Department>()
                .Where(d => d.Active)
                .JoinQueryOver<Company>(d => d.Company)
                .Where(c => c.Active);

            if (pageNum == -1 & pageSize == -1) {

                return new QueryResult<Department>(query?
                    .List().AsQueryable());
            }

            return new QueryResult<Department>(query
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize);
        }

        public QueryResult<Department> FindAllActiveByCompanyPagedOf(long companyId, int? pageNum, int? pageSize) {
            var query = this.Session.QueryOver<Department>()
                .Where(d => d.Active)
                .JoinQueryOver<Company>(d => d.Company)
                .Where(c => c.Active)
                .And(c => c.Id == companyId);

            if (pageNum == -1 & pageSize == -1)
            {

                return new QueryResult<Department>(query?
                    .List().AsQueryable());
            }

            return new QueryResult<Department>(query
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize);
        }

        public Department FindActiveByName(string name) {
            Department dep = null;
            Company com = null;

            return this.Session.QueryOver<Department>(() => dep)
                .JoinEntityAlias(() => com, () => dep.Company.Id == com.Id)
                .Where(() => dep.Active == true)
                .And(() => com.Active)
                .And(() => dep.Name == name)
                .Cacheable()
                .CacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Manual)
                .SingleOrDefault();
        }

        public Department FindActiveById(long id) {
            Department dep = null;
            Company com = null;

            return this.Session.QueryOver<Department>(() => dep)
                .JoinEntityAlias(() => com, () => dep.Company.Id == com.Id)
                .Where(() => dep.Active == true)
                .And(() => com.Active)
                .And(() => dep.Id == id)
                .Cacheable()
                .CacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Manual)
                .SingleOrDefault();
        }
    }
}
