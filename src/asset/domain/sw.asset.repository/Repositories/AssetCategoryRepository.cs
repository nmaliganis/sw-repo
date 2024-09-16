using sw.asset.contracts.ContractRepositories;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.asset.model.Assets.Categories;
using NHibernate;

namespace sw.asset.repository.Repositories
{
    public class AssetCategoryRepository : RepositoryBase<AssetCategory, long>, IAssetCategoryRepository
    {
        public AssetCategoryRepository(ISession session)
            : base(session)
        {
        }

        public QueryResult<AssetCategory> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<AssetCategory>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<AssetCategory>(query?
                  .Where(ac => ac.Active == true)
                  .List().AsQueryable());
            }

            return new QueryResult<AssetCategory>(query
                  .Where(ac => ac.Active == true)
                  .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                  .Take((int)pageSize).List().AsQueryable(),
                query.ToRowCountQuery().RowCount(),
                (int)pageSize)
              ;
        }

        public AssetCategory FindActiveById(long id)
        {
            AssetCategory ac = null;

            return this.Session.QueryOver<AssetCategory>(() => ac)
                .Where(() => ac.Active == true)
                .And(() => ac.Id == id)
                .Cacheable()
                .CacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Manual)
                .SingleOrDefault();
        }
    }// Class: AssetCategoryRepository
}// Namespace: sw.asset.repository.Repositories
