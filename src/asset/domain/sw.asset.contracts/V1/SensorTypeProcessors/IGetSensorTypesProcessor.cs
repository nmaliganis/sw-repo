using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.SensorTypeProcessors
{
    public interface IGetSensorTypesProcessor
    {
        Task<BusinessResult<PagedList<SensorTypeUiModel>>> GetSensorTypesAsync(GetSensorTypesQuery qry);
    }
}
