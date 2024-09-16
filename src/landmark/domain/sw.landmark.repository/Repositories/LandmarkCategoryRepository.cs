using sw.landmark.contracts.ContractRepositories;
using sw.landmark.model;
using sw.landmark.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.landmark.repository.DbContexts;

namespace sw.landmark.repository.Repositories
{
    public class LandmarkCategoryRepository : RepositoryBase<LandmarkCategory, long>, ILandmarkCategoryRepository
    {
        public LandmarkCategoryRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<LandmarkCategory> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from lac in Context.LandmarkCategories
                      where lac.Active
                      select lac;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<LandmarkCategory>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<LandmarkCategory>(result, qry.Count(), (int)pageSize);
        }

        public LandmarkCategory FindActiveBy(long id)
        {
            var qry = from lac in Context.LandmarkCategories
                      where lac.Active
                         && lac.Id == id
                      select lac;

            return qry.FirstOrDefault();
        }
    }
}
