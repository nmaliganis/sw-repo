using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Companies.Zones;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System.Linq;
using sw.infrastructure.Paging;

namespace sw.asset.repository.Repositories;

public class ZoneRepository : RepositoryBase<Zone, long>, IZoneRepository
{
    public ZoneRepository(ISession session)
        : base(session)
    {
    }

    public QueryResult<Zone> FindAllActivePagedOf(long companyId, int? pageNum, int? pageSize)
    {
        var criteria = this.Session.CreateCriteria<Zone>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Zone>(criteria
                .CreateAlias("Company", "c", JoinType.InnerJoin)
                .Add(Expression.Eq("Active", true))
                .Add(Expression.Eq("c.Active", true))
                .List<Zone>()
                .AsQueryable());
        }

        return new QueryResult<Zone>(criteria
            .CreateAlias("Company", "c", JoinType.InnerJoin)
            .Add(Expression.Eq("Active", true))
            .Add(Expression.Eq("c.Active", true))
            .SetFirstResult(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
            .SetMaxResults((int)pageSize)
            .List<Zone>().AsQueryable(),
                criteria.SetProjection(Projections.RowCount()).UniqueResult<int>(),
                (int)pageSize)
        ;
    }

    public Zone FindActiveById(long id)
    {
        return (Zone)
            Session.CreateCriteria(typeof(Zone))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("Active", true))
                .UniqueResult()
            ;
    }

    public Zone FindActiveByName(string name)
    {
        return (Zone)
            Session.CreateCriteria(typeof(Zone))
                .Add(Restrictions.Eq("Name", name))
                .Add(Restrictions.Eq("Active", true))
                .UniqueResult()
            ;
    }
}//Class : ZoneRepository