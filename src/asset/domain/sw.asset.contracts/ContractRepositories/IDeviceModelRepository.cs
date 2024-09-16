using sw.asset.model.Devices;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories
{
    public interface IDeviceModelRepository : IRepository<DeviceModel, long>
    {
        QueryResult<DeviceModel> FindAllActivePagedOf(int? pageNum, int? pageSize);
        DeviceModel FindActiveById(long id);
    }
}
