using sw.asset.model.SensorTypes;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories;

public interface ISensorTypeRepository : IRepository<SensorType, long>
{
    QueryResult<SensorType> FindAllActivePagedOf(int? pageNum, int? pageSize);
    SensorType FindActiveById(long id);
    SensorType FindSensorTypeByName(string name);
    SensorType FindBySensorTypeIndex(int sensorTypeIndex);
}// Class: ISensorTypeRepository