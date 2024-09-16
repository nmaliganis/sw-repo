using sw.asset.common.dtos.Vms.Sensors;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Sensor;

namespace sw.asset.contracts.V1.SensorProcessors
{
    public interface IUpdateSensorProcessor
    {
        Task<BusinessResult<SensorModificationUiModel>> UpdateSensorAsync(UpdateSensorCommand updateCommand);
    }
}
