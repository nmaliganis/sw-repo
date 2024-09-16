using System.Linq;
using sw.auth.contracts.ContractRepositories;
using sw.auth.model.Companies;
using sw.auth.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;

namespace sw.auth.repository.Repositories;

public class CompanyRepository : RepositoryBase<Company, long>, ICompanyRepository {
    public CompanyRepository(ISession session)
        : base(session) {
    }

    public QueryResult<Company> FindAllActiveCompaniesPagedOf(int? pageNum, int? pageSize) {
        var query = Session.QueryOver<Company>();

        if (pageNum == -1 & pageSize == -1) {
            return new QueryResult<Company>(query?
                .Where(r => r.Active == true)
                .List().AsQueryable());
        }

        return new QueryResult<Company>(query
                    .Where(r => r.Active == true)
                    .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                    .Take((int)pageSize).List().AsQueryable(),
                query.ToRowCountQuery().RowCount(),
                (int)pageSize)
            ;
    }

    public QueryResult<Company> FindAllActiveCompaniesByUserPagedOf(long companyId, int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<Company>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Company>(query?
                .Where(r => r.Active == true)
                .Where(r => r.Id == companyId)
                .List().AsQueryable());
        }

        return new QueryResult<Company>(query
                    .Where(r => r.Active == true)
                    .Where(r => r.Id == companyId)
                    .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                    .Take((int)pageSize).List().AsQueryable(),
                query.ToRowCountQuery().RowCount(),
                (int)pageSize)
            ;
    }

    public int FindCountAllActiveCompanies() {
        int count;

        count = Session
            .CreateCriteria<Company>()
            .Add(Expression.Eq("Active", true))
            .SetProjection(
                Projections.Count(Projections.Id())
            )
            .UniqueResult<int>();
        return count;
    }

    public Company FindCompanyByName(string name) {
        return
            (Company)
            Session.CreateCriteria(typeof(Company))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Active", true))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .UniqueResult()
            ;
    }

    public Company FindActiveById(long id)
    {
        Company com = null;

        return this.Session.QueryOver<Company>(() => com)
            .Where(() => com.Active == true)
            .And(() => com.Id == id)
            .Cacheable()
            .CacheMode(CacheMode.Normal)
            .SetFlushMode(FlushMode.Manual)
            .SingleOrDefault();
    }
}