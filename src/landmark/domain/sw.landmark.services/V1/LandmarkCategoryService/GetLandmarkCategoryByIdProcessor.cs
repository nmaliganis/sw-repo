using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkCategoryProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkCategoryService
{
    public class GetLandmarkCategoryByIdProcessor :
        IGetLandmarkCategoryByIdProcessor,
        IRequestHandler<GetLandmarkCategoryByIdQuery, BusinessResult<LandmarkCategoryUiModel>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ILandmarkCategoryRepository _landmarkCategoryRepository;
        public GetLandmarkCategoryByIdProcessor(ILandmarkCategoryRepository landmarkCategoryRepository, IAutoMapper autoMapper)
        {
            _landmarkCategoryRepository = landmarkCategoryRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<LandmarkCategoryUiModel>> GetLandmarkCategoryByIdAsync(long id)
        {
            var bc = new BusinessResult<LandmarkCategoryUiModel>(new LandmarkCategoryUiModel());

            var landmarkCategory = _landmarkCategoryRepository.FindActiveBy(id);
            if (landmarkCategory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "LandmarkCategory Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<LandmarkCategoryUiModel>(landmarkCategory);
            response.Message = $"LandmarkCategory id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<LandmarkCategoryUiModel>> Handle(GetLandmarkCategoryByIdQuery qry, CancellationToken cancellationToken)
        {
            return await GetLandmarkCategoryByIdAsync(qry.Id);
        }
    }
}
