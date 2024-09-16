using sw.asset.model.Devices;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories;

public interface IDeviceRepository : IRepository<Device, long>
{
  QueryResult<Device> FindAllActivePagedOf(int? pageNum, int? pageSize);
  Device FindOneByImei(string imei);
  Device FindOneBySerialNumber(string serialNumber);
  Device FindActiveById(long id);
  int FindCountAllActive();
}