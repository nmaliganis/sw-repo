using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocoderProfileProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.GeocoderProfileService
{
    public class DeleteHardGeocoderProfileProcessor :
        IDeleteHardGeocoderProfileProcessor,
        IRequestHandler<DeleteHardGeocoderProfileCommand, BusinessResult<GeocoderProfileDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocoderProfileRepository _geocoderProfileRepository;

        public DeleteHardGeocoderProfileProcessor(IUnitOfWork uOf, IGeocoderProfileRepository geocoderProfileRepository)
        {
            _uOf = uOf;
            _geocoderProfileRepository = geocoderProfileRepository;
        }

        public async Task<BusinessResult<GeocoderProfileDeletionUiModel>> Handle(DeleteHardGeocoderProfileCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteHardGeocoderProfileAsync(deleteCommand);
        }

        public async Task<BusinessResult<GeocoderProfileDeletionUiModel>> DeleteHardGeocoderProfileAsync(DeleteHardGeocoderProfileCommand deleteCommand)
        {
            var bc = new BusinessResult<GeocoderProfileDeletionUiModel>(new GeocoderProfileDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var geocoderProfile = _geocoderProfileRepository.FindBy(deleteCommand.Id);
            if (geocoderProfile is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "GeocoderProfile Id does not exist"));
                return bc;
            }

            Persist(geocoderProfile);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"GeocoderProfile with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(GeocoderProfile geocoderProfile)
        {
            _geocoderProfileRepository.Remove(geocoderProfile);
            _uOf.Commit();
        }
    }
}
