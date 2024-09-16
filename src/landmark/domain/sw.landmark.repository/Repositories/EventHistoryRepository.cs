using sw.landmark.contracts.ContractRepositories;
using sw.landmark.model;
using sw.landmark.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.landmark.repository.DbContexts;

namespace sw.landmark.repository.Repositories
{
    public class EventHistoryRepository : RepositoryBase<EventHistory, long>, IEventHistoryRepository
    {
        public EventHistoryRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<EventHistory> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from evh in Context.EventHistories
                      join evp in Context.EventPositions on evh.EventPositionId equals evp.Id
                      join gpo in Context.GeocodedPositions on evp.GeocodedPositionId equals gpo.Id
                      join gpr in Context.GeocoderProfiles on gpo.GeocoderProfileId equals gpr.Id
                      where evh.Active
                         && evp.Active
                         && gpo.Active
                         && gpr.Active
                      select evh;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<EventHistory>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<EventHistory>(result, qry.Count(), (int)pageSize);
        }

        public EventHistory FindActiveBy(long id)
        {
            var qry = from evh in Context.EventHistories
                      join evp in Context.EventPositions on evh.EventPositionId equals evp.Id
                      join gpo in Context.GeocodedPositions on evp.GeocodedPositionId equals gpo.Id
                      join gpr in Context.GeocoderProfiles on gpo.GeocoderProfileId equals gpr.Id
                      where evh.Active
                         && evp.Active
                         && gpo.Active
                         && gpr.Active
                         && evh.Id == id
                      select evh;

            return qry.FirstOrDefault();
        }
    }
}
