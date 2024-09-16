using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.GeocoderProfileProcessors
{
    public interface IGetGeocoderProfileByIdProcessor
    {
        Task<BusinessResult<GeocoderProfileUiModel>> GetGeocoderProfileByIdAsync(long id);
    }
}
