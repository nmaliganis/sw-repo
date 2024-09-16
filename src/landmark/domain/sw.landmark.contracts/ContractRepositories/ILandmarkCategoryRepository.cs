using sw.landmark.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.landmark.contracts.ContractRepositories
{
    public interface ILandmarkCategoryRepository : IRepository<LandmarkCategory, long>
    {
        QueryResult<LandmarkCategory> FindAllActivePagedOf(int? pageNum, int? pageSize);
        LandmarkCategory FindActiveBy(long id);
    }
}
