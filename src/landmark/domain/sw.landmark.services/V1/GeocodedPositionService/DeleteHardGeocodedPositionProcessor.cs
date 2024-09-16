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
    public class DeleteHardGeocodedPositionProcessor :
        IDeleteHardGeocodedPositionProcessor,
        IRequestHandler<DeleteHardGeocodedPositionCommand, BusinessResult<GeocodedPositionDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocodedPositionRepository _geocodedPositionRepository;

        public DeleteHardGeocodedPositionProcessor(IUnitOfWork uOf, IGeocodedPositionRepository geocodedPositionRepository)
        {
            _uOf = uOf;
            _geocodedPositionRepository = geocodedPositionRepository;
        }

        public async Task<BusinessResult<GeocodedPositionDeletionUiModel>> Handle(DeleteHardGeocodedPositionCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteHardGeocodedPositionAsync(deleteCommand);
        }

        public async Task<BusinessResult<GeocodedPositionDeletionUiModel>> DeleteHardGeocodedPositionAsync(DeleteHardGeocodedPositionCommand deleteCommand)
        {
            var bc = new BusinessResult<GeocodedPositionDeletionUiModel>(new GeocodedPositionDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var geocodedPosition = _geocodedPositionRepository.FindBy(deleteCommand.Id);
            if (geocodedPosition is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "GeocodedPosition Id does not exist"));
                return bc;
            }

            Persist(geocodedPosition);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"GeocodedPosition with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(GeocodedPosition geocodedPosition)
        {
            _geocodedPositionRepository.Remove(geocodedPosition);
            _uOf.Commit();
        }
    }
}
