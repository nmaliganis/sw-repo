using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkCategoryProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkCategoryService
{
    public class UpdateLandmarkCategoryProcessor :
        IUpdateLandmarkCategoryProcessor,
        IRequestHandler<UpdateLandmarkCategoryCommand, BusinessResult<LandmarkCategoryModificationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILandmarkCategoryRepository _landmarkCategoryRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateLandmarkCategoryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ILandmarkCategoryRepository landmarkCategoryRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _landmarkCategoryRepository = landmarkCategoryRepository;
        }

        public async Task<BusinessResult<LandmarkCategoryModificationUiModel>> Handle(UpdateLandmarkCategoryCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateLandmarkCategoryAsync(updateCommand);
        }

        public async Task<BusinessResult<LandmarkCategoryModificationUiModel>> UpdateLandmarkCategoryAsync(UpdateLandmarkCategoryCommand updateCommand)
        {
            var bc = new BusinessResult<LandmarkCategoryModificationUiModel>(new LandmarkCategoryModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var landmarkCategory = _landmarkCategoryRepository.FindBy(updateCommand.Id);
            if (landmarkCategory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "LandmarkCategory Id does not exist"));
                return bc;
            }

            var modifiedLandmarkCategory = _autoMapper.Map<LandmarkCategory>(updateCommand);
            landmarkCategory.Modified(updateCommand.ModifiedById, modifiedLandmarkCategory);

            Persist(landmarkCategory, updateCommand.Id);

            var response = _autoMapper.Map<LandmarkCategoryModificationUiModel>(landmarkCategory);
            response.Message = $"LandmarkCategory id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(LandmarkCategory landmarkCategory, long id)
        {
            _landmarkCategoryRepository.Save(landmarkCategory, id);
            _uOf.Commit();
        }
    }
}
