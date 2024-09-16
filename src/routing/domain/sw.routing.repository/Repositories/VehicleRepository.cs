using System.Linq;
using sw.routing.contracts.ContractRepositories;
using sw.routing.model.Vehicles;
using sw.routing.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;

namespace sw.routing.repository.Repositories
{
    public class VehicleRepository : RepositoryBase<Vehicle, long>, IVehicleRepository
    {
        public VehicleRepository(ISession session)
            : base(session)
        {
        }

        public int FindCountAllActiveVehicles()
        {
            var count = Session
                .CreateCriteria<Vehicle>()
                .Add(Expression.Eq("Active", true))
                .SetProjection(
                    Projections.Count(Projections.Id())
                )
                .UniqueResult<int>();
            return count;
        }


        public QueryResult<Vehicle> FindAllActiveVehiclesPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Vehicle>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Vehicle>(query?
                    .Where(r => r.Active == true)
                    .List().AsQueryable());
            }

            return new QueryResult<Vehicle>(query
                        .Where(r => r.Active == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public Vehicle FindVehicleByNumPlate(string numPlate)
        {
            return
                (Vehicle)
                Session.CreateCriteria(typeof(Vehicle))
                    .Add(Expression.Eq("NumPlate", numPlate))
                    .Add(Expression.Eq("Active", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .UniqueResult()
                ;
        }
    }
}