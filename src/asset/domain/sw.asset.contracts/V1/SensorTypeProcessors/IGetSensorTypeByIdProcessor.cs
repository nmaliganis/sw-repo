using sw.asset.common.dtos.Vms.SensorTypes;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.SensorTypeProcessors
{
    public interface IGetSensorTypeByIdProcessor
    {
        Task<BusinessResult<SensorTypeUiModel>> GetSensorTypeByIdAsync(long id);
    }
}
