using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocodedPositionProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.GeocodedPositionService
{
    public class DeleteSoftGeocodedPositionProcessor :
        IDeleteSoftGeocodedPositionProcessor,
        IRequestHandler<DeleteSoftGeocodedPositionCommand, BusinessResult<GeocodedPositionDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocodedPositionRepository _geocodedPositionRepository;

        public DeleteSoftGeocodedPositionProcessor(IUnitOfWork uOf, IGeocodedPositionRepository geocodedPositionRepository)
        {
            _uOf = uOf;
            _geocodedPositionRepository = geocodedPositionRepository;
        }

        public async Task<BusinessResult<GeocodedPositionDeletionUiModel>> Handle(DeleteSoftGeocodedPositionCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteSoftGeocodedPositionAsync(deleteCommand);
        }

        public async Task<BusinessResult<GeocodedPositionDeletionUiModel>> DeleteSoftGeocodedPositionAsync(DeleteSoftGeocodedPositionCommand deleteCommand)
        {
            var bc = new BusinessResult<GeocodedPositionDeletionUiModel>(new GeocodedPositionDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var geocodedPosition = _geocodedPositionRepository.FindBy(deleteCommand.Id);
            if (geocodedPosition is null || !geocodedPosition.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "GeocodedPosition Id does not exist"));
                return bc;
            }

            geocodedPosition.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(geocodedPosition, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"GeocodedPosition with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(GeocodedPosition geocodedPosition, long id)
        {
            _geocodedPositionRepository.Save(geocodedPosition, id);
            _uOf.Commit();
        }
    }
}
