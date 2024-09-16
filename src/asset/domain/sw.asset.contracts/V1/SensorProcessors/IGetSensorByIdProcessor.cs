using sw.asset.common.dtos.Vms.Sensors;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.SensorProcessors
{
    public interface IGetSensorByIdProcessor
    {
        Task<BusinessResult<SensorUiModel>> GetSensorByIdAsync(long id);
    }
}
