using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocoderProfileProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.GeocoderProfileService
{
    public class GetGeocoderProfileByIdProcessor :
        IGetGeocoderProfileByIdProcessor,
        IRequestHandler<GetGeocoderProfileByIdQuery, BusinessResult<GeocoderProfileUiModel>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IGeocoderProfileRepository _geocoderProfileRepository;
        public GetGeocoderProfileByIdProcessor(IGeocoderProfileRepository geocoderProfileRepository, IAutoMapper autoMapper)
        {
            _geocoderProfileRepository = geocoderProfileRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<GeocoderProfileUiModel>> GetGeocoderProfileByIdAsync(long id)
        {
            var bc = new BusinessResult<GeocoderProfileUiModel>(new GeocoderProfileUiModel());

            var geocoderProfile = _geocoderProfileRepository.FindActiveBy(id);
            if (geocoderProfile is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "GeocoderProfile Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<GeocoderProfileUiModel>(geocoderProfile);
            response.Message = $"GeocoderProfile id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<GeocoderProfileUiModel>> Handle(GetGeocoderProfileByIdQuery qry, CancellationToken cancellationToken)
        {
            return await GetGeocoderProfileByIdAsync(qry.Id);
        }
    }
}
