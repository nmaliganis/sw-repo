using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.GeocodedPositionProcessors
{
    public interface IDeleteHardGeocodedPositionProcessor
    {
        Task<BusinessResult<GeocodedPositionDeletionUiModel>> DeleteHardGeocodedPositionAsync(DeleteHardGeocodedPositionCommand deleteCommand);
    }
}
