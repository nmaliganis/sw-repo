using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.SensorProcessors;

public interface ICreateSensorProcessor
{
  Task<BusinessResult<SensorUiModel>> CreateSensorAsync(CreateSensorCommand createCommand);
  Task<BusinessResult<SensorUiModel>> CreateSensorByImeiAsync(CreateSensorByDeviceImeiCommand createCommand);
}