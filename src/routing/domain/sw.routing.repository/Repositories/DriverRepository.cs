using System.Linq;
using sw.routing.contracts.ContractRepositories;
using sw.routing.model.Drivers;
using sw.routing.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;

namespace sw.routing.repository.Repositories
{
    public class DriverRepository : RepositoryBase<Driver, long>, IDriverRepository
    {
        public DriverRepository(ISession session)
            : base(session)
        {
        }

        public int FindCountAllActiveDrivers()
        {
            var count = Session
                .CreateCriteria<Driver>()
                .Add(Expression.Eq("Active", true))
                .SetProjection(
                    Projections.Count(Projections.Id())
                )
                .UniqueResult<int>();
            return count;
        }


        public QueryResult<Driver> FindAllActiveDriversPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Driver>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Driver>(query?
                    .Where(r => r.Active == true)
                    .List().AsQueryable());
            }

            return new QueryResult<Driver>(query
                        .Where(r => r.Active == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public Driver FindDriverByEmail(string email)
        {
            return
                (Driver)
                Session.CreateCriteria(typeof(Driver))
                    .Add(Expression.Eq("Email", email))
                    .Add(Expression.Eq("Active", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;
        }
    }
}