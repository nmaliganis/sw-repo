using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocodedPositionProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.GeocodedPositionService
{
    public class GetGeocodedPositionByIdProcessor :
        IGetGeocodedPositionByIdProcessor,
        IRequestHandler<GetGeocodedPositionByIdQuery, BusinessResult<GeocodedPositionUiModel>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IGeocodedPositionRepository _geocodedPositionRepository;
        public GetGeocodedPositionByIdProcessor(IGeocodedPositionRepository geocodedPositionRepository, IAutoMapper autoMapper)
        {
            _geocodedPositionRepository = geocodedPositionRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<GeocodedPositionUiModel>> GetGeocodedPositionByIdAsync(long id)
        {
            var bc = new BusinessResult<GeocodedPositionUiModel>(new GeocodedPositionUiModel());

            var geocodedPosition = _geocodedPositionRepository.FindActiveBy(id);
            if (geocodedPosition is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "GeocodedPosition Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<GeocodedPositionUiModel>(geocodedPosition);
            response.Message = $"GeocodedPosition id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<GeocodedPositionUiModel>> Handle(GetGeocodedPositionByIdQuery qry, CancellationToken cancellationToken)
        {
            return await GetGeocodedPositionByIdAsync(qry.Id);
        }
    }
}
