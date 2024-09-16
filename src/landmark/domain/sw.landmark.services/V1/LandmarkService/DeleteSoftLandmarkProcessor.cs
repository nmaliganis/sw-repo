using sw.landmark.common.dtos.V1.Cqrs.Landmarks;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkService
{
    public class DeleteSoftLandmarkProcessor :
        IDeleteSoftLandmarkProcessor,
        IRequestHandler<DeleteSoftLandmarkCommand, BusinessResult<LandmarkDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILandmarkRepository _landmarkRepository;

        public DeleteSoftLandmarkProcessor(IUnitOfWork uOf, ILandmarkRepository landmarkRepository)
        {
            _uOf = uOf;
            _landmarkRepository = landmarkRepository;
        }

        public async Task<BusinessResult<LandmarkDeletionUiModel>> Handle(DeleteSoftLandmarkCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteSoftLandmarkAsync(deleteCommand);
        }

        public async Task<BusinessResult<LandmarkDeletionUiModel>> DeleteSoftLandmarkAsync(DeleteSoftLandmarkCommand deleteCommand)
        {
            var bc = new BusinessResult<LandmarkDeletionUiModel>(new LandmarkDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var landmark = _landmarkRepository.FindBy(deleteCommand.Id);
            if (landmark is null || !landmark.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Landmark Id does not exist"));
                return bc;
            }

            landmark.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(landmark, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"Landmark with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Landmark landmark, long id)
        {
            _landmarkRepository.Save(landmark, id);
            _uOf.Commit();
        }
    }
}
