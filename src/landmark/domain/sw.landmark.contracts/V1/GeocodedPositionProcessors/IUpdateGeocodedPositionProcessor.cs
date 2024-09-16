using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.GeocodedPositionProcessors
{
    public interface IUpdateGeocodedPositionProcessor
    {
        Task<BusinessResult<GeocodedPositionModificationUiModel>> UpdateGeocodedPositionAsync(UpdateGeocodedPositionCommand updateCommand);
    }
}
