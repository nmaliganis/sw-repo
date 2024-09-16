using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.GeocodedPositionProcessors
{
    public interface ICreateGeocodedPositionProcessor
    {
        Task<BusinessResult<GeocodedPositionCreationUiModel>> CreateGeocodedPositionAsync(CreateGeocodedPositionCommand createCommand);
    }
}
