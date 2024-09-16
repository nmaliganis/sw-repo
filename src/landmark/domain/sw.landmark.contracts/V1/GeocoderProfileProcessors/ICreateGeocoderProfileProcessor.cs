using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.GeocoderProfileProcessors
{
    public interface ICreateGeocoderProfileProcessor
    {
        Task<BusinessResult<GeocoderProfileCreationUiModel>> CreateGeocoderProfileAsync(CreateGeocoderProfileCommand createCommand);
    }
}
