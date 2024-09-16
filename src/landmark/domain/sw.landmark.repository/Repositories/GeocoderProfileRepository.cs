using sw.landmark.contracts.ContractRepositories;
using sw.landmark.model;
using sw.landmark.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.landmark.repository.DbContexts;

namespace sw.landmark.repository.Repositories
{
    public class GeocoderProfileRepository : RepositoryBase<GeocoderProfile, long>, IGeocoderProfileRepository
    {
        public GeocoderProfileRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<GeocoderProfile> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from gpr in Context.GeocoderProfiles
                      where gpr.Active
                      select gpr;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<GeocoderProfile>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<GeocoderProfile>(result, qry.Count(), (int)pageSize);
        }

        public GeocoderProfile FindActiveBy(long id)
        {
            var qry = from gpr in Context.GeocoderProfiles
                      where gpr.Active
                         && gpr.Id == id
                      select gpr;

            return qry.FirstOrDefault();
        }
    }
}
