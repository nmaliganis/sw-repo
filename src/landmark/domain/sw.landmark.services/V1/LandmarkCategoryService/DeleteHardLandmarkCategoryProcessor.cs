using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkCategoryProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkCategoryService
{
    public class DeleteHardLandmarkCategoryProcessor :
        IDeleteHardLandmarkCategoryProcessor,
        IRequestHandler<DeleteHardLandmarkCategoryCommand, BusinessResult<LandmarkCategoryDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILandmarkCategoryRepository _landmarkCategoryRepository;

        public DeleteHardLandmarkCategoryProcessor(IUnitOfWork uOf, ILandmarkCategoryRepository landmarkCategoryRepository)
        {
            _uOf = uOf;
            _landmarkCategoryRepository = landmarkCategoryRepository;
        }

        public async Task<BusinessResult<LandmarkCategoryDeletionUiModel>> Handle(DeleteHardLandmarkCategoryCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteHardLandmarkCategoryAsync(deleteCommand);
        }

        public async Task<BusinessResult<LandmarkCategoryDeletionUiModel>> DeleteHardLandmarkCategoryAsync(DeleteHardLandmarkCategoryCommand deleteCommand)
        {
            var bc = new BusinessResult<LandmarkCategoryDeletionUiModel>(new LandmarkCategoryDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var landmarkCategory = _landmarkCategoryRepository.FindBy(deleteCommand.Id);
            if (landmarkCategory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "LandmarkCategory Id does not exist"));
                return bc;
            }

            Persist(landmarkCategory);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"LandmarkCategory with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(LandmarkCategory landmarkCategory)
        {
            _landmarkCategoryRepository.Remove(landmarkCategory);
            _uOf.Commit();
        }
    }
}
