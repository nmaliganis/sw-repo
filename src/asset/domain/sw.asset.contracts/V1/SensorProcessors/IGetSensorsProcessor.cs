using sw.asset.common.dtos.Vms.Sensors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Sensor;

namespace sw.asset.contracts.V1.SensorProcessors
{
    public interface IGetSensorsProcessor
    {
        Task<BusinessResult<PagedList<SensorUiModel>>> GetSensorsAsync(GetSensorsQuery qry);
    }
}
