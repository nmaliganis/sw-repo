using sw.landmark.contracts.ContractRepositories;
using sw.landmark.model;
using sw.landmark.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.landmark.repository.DbContexts;

namespace sw.landmark.repository.Repositories
{
    public class LandmarkRepository : RepositoryBase<Landmark, long>, ILandmarkRepository
    {
        public LandmarkRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<Landmark> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from lan in Context.Landmarks
                      join lac in Context.LandmarkCategories on lan.LandmarkCategoryId equals lac.Id
                      where lan.Active
                         && lac.Active
                      select lan;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Landmark>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<Landmark>(result, qry.Count(), (int)pageSize);
        }

        public Landmark FindActiveBy(long id)
        {
            var qry = from lan in Context.Landmarks
                      join lac in Context.LandmarkCategories on lan.LandmarkCategoryId equals lac.Id
                      where lan.Active
                         && lac.Active
                         && lan.Id == id
                      select lan;

            return qry.FirstOrDefault();
        }
    }
}
