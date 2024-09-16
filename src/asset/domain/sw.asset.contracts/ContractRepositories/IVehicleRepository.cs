using sw.asset.model.Assets.Vehicles;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories
{
    public interface IVehicleRepository : IRepository<Vehicle, long>
    {
        QueryResult<Vehicle> FindAllActivePagedOf(int? pageNum, int? pageSize);
        Vehicle FindActiveBy(long id);
    }
}
