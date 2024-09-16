using sw.landmark.common.dtos.V1.Cqrs.Landmarks;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkService
{
    public class GetLandmarkByIdProcessor :
        IGetLandmarkByIdProcessor,
        IRequestHandler<GetLandmarkByIdQuery, BusinessResult<LandmarkUiModel>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ILandmarkRepository _landmarkRepository;
        public GetLandmarkByIdProcessor(ILandmarkRepository landmarkRepository, IAutoMapper autoMapper)
        {
            _landmarkRepository = landmarkRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<LandmarkUiModel>> GetLandmarkByIdAsync(long id)
        {
            var bc = new BusinessResult<LandmarkUiModel>(new LandmarkUiModel());

            var landmark = _landmarkRepository.FindActiveBy(id);
            if (landmark is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Landmark Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<LandmarkUiModel>(landmark);
            response.Message = $"Landmark id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<LandmarkUiModel>> Handle(GetLandmarkByIdQuery qry, CancellationToken cancellationToken)
        {
            return await GetLandmarkByIdAsync(qry.Id);
        }
    }
}
