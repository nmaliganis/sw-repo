using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Assets.Categories;
using sw.asset.model.Assets.Vehicles;
using sw.asset.model.Companies;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using System.Linq;

namespace sw.asset.repository.Repositories
{
    public class VehicleRepository : RepositoryBase<Vehicle, long>, IVehicleRepository
    {
        private Company company;
        private AssetCategory category;

        public VehicleRepository(ISession session) : base(session)
        {
        }

        public QueryResult<Vehicle> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Vehicle>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Vehicle>(query?
                  .Where(sn => sn.Active == true)
                  .List().AsQueryable());
            }
            return new QueryResult<Vehicle>(query
                .JoinAlias(veh => veh.Company, () => company)
                .JoinAlias(veh => veh.AssetCategory, () => category)
                .Where(veh => veh.Active == true)
                .And(() => company.Active == true)
                .And(() => category.Active == true)
                .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                .Take((int)pageSize).List().AsQueryable(),
                query.ToRowCountQuery().RowCount(),
                (int)pageSize)
              ;
        }

        public Vehicle FindActiveBy(long id)
        {
            Vehicle veh = null;

            return this.Session.QueryOver<Vehicle>(() => veh)
                .JoinAlias(veh => veh.Company, () => company)
                .JoinAlias(veh => veh.AssetCategory, () => category)
                .Where(veh => veh.Active == true)
                .And(() => company.Active == true)
                .And(() => category.Active == true)
                .And(() => veh.Id == id)
                .Cacheable()
                .CacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Manual)
                .SingleOrDefault();
        }
    }// Class: VehicleRepository
}// Namespace: sw.asset.repository.Repositories
