using sw.routing.contracts.ContractRepositories;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;
using sw.routing.model.Itineraries;
using sw.routing.repository.Repositories.Base;

namespace sw.routing.repository.Repositories;

public class ItineraryRepository : RepositoryBase<Itinerary, long>, IItineraryRepository
{
    public ItineraryRepository(ISession session)
      : base(session)
    {
    }

    public QueryResult<Itinerary> FindAllActiveItinerariesPagedOf(int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<Itinerary>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Itinerary>(query?
              .Where(r => r.Active == true)
              .List().AsQueryable());
        }

        return new QueryResult<Itinerary>(query
              .Where(r => r.Active == true)
              .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
              .Take((int)pageSize).List().AsQueryable(),
            query.ToRowCountQuery().RowCount(),
            (int)pageSize)
          ;
    }

    public int FindCountAllActiveItineraries()
    {
        var count = Session
            .CreateCriteria<Itinerary>()
            .Add(Expression.Eq("Active", true))
            .SetProjection(
                Projections.Count(Projections.Id())
            )
            .UniqueResult<int>();

        return count;
    }

    public Itinerary FindItineraryByName(string name)
    {
        return
          (Itinerary)
          Session.CreateCriteria(typeof(Itinerary))
            .Add(Expression.Eq("Name", name))
            .Add(Expression.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .UniqueResult()
          ;
    }
}