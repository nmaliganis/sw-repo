using System.Collections.Generic;
using sw.asset.model.Sensors;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories;

public interface ISensorRepository : IRepository<Sensor, long>
{
    QueryResult<Sensor> FindAllActivePagedOf(int? pageNum, int? pageSize);
    Sensor FindActiveById(long id);
    Sensor FindOneByName(string name);
    Sensor FindOneByNameAndDeviceId(string name, long deviceId);
    IList<Sensor> FindByTypeAndDeviceImei(string sensorTypeName, string deviceImei);
    IList<Sensor> FindByDeviceImei(string deviceImei);
    IList<Sensor> FindByDeviceImeiAndSensorTypeIndex(string deviceImei, int sensorTypeIndex);
    IList<Sensor> FindByAllByContainerId(long containerId);
}