using sw.landmark.contracts.ContractRepositories;
using sw.landmark.model;
using sw.landmark.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.landmark.repository.DbContexts;

namespace sw.landmark.repository.Repositories
{
    public class GeocodedPositionRepository : RepositoryBase<GeocodedPosition, long>, IGeocodedPositionRepository
    {
        public GeocodedPositionRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<GeocodedPosition> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from gpo in Context.GeocodedPositions
                      join gpr in Context.GeocoderProfiles on gpo.GeocoderProfileId equals gpr.Id
                      where gpo.Active
                         && gpr.Active
                      select gpo;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<GeocodedPosition>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<GeocodedPosition>(result, qry.Count(), (int)pageSize);
        }

        public GeocodedPosition FindActiveBy(long id)
        {
            var qry = from gpo in Context.GeocodedPositions
                      join gpr in Context.GeocoderProfiles on gpo.GeocoderProfileId equals gpr.Id
                      where gpo.Active
                         && gpr.Active
                         && gpo.Id == id
                      select gpo;

            return qry.FirstOrDefault();
        }
    }
}
