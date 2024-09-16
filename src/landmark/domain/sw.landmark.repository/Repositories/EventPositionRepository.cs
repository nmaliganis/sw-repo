using sw.landmark.contracts.ContractRepositories;
using sw.landmark.model;
using sw.landmark.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.landmark.repository.DbContexts;

namespace sw.landmark.repository.Repositories
{
    public class EventPositionRepository : RepositoryBase<EventPosition, long>, IEventPositionRepository
    {
        public EventPositionRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<EventPosition> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from evp in Context.EventPositions
                      join gpo in Context.GeocodedPositions on evp.GeocodedPositionId equals gpo.Id
                      join gpr in Context.GeocoderProfiles on gpo.GeocoderProfileId equals gpr.Id
                      where evp.Active
                         && gpo.Active
                         && gpr.Active
                      select evp;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<EventPosition>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<EventPosition>(result, qry.Count(), (int)pageSize);
        }

        public EventPosition FindActiveBy(long id)
        {
            var qry = from evp in Context.EventPositions
                      join gpo in Context.GeocodedPositions on evp.GeocodedPositionId equals gpo.Id
                      join gpr in Context.GeocoderProfiles on gpo.GeocoderProfileId equals gpr.Id
                      where evp.Active
                         && gpo.Active
                         && gpr.Active
                         && evp.Id == id
                      select evp;

            return qry.FirstOrDefault();
        }
    }
}
