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
    public class DeleteSoftGeocoderProfileProcessor :
        IDeleteSoftGeocoderProfileProcessor,
        IRequestHandler<DeleteSoftGeocoderProfileCommand, BusinessResult<GeocoderProfileDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocoderProfileRepository _geocoderProfileRepository;

        public DeleteSoftGeocoderProfileProcessor(IUnitOfWork uOf, IGeocoderProfileRepository geocoderProfileRepository)
        {
            _uOf = uOf;
            _geocoderProfileRepository = geocoderProfileRepository;
        }

        public async Task<BusinessResult<GeocoderProfileDeletionUiModel>> Handle(DeleteSoftGeocoderProfileCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteSoftGeocoderProfileAsync(deleteCommand);
        }

        public async Task<BusinessResult<GeocoderProfileDeletionUiModel>> DeleteSoftGeocoderProfileAsync(DeleteSoftGeocoderProfileCommand deleteCommand)
        {
            var bc = new BusinessResult<GeocoderProfileDeletionUiModel>(new GeocoderProfileDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var geocoderProfile = _geocoderProfileRepository.FindBy(deleteCommand.Id);
            if (geocoderProfile is null || !geocoderProfile.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "GeocoderProfile Id does not exist"));
                return bc;
            }

            geocoderProfile.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(geocoderProfile, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"GeocoderProfile with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(GeocoderProfile geocoderProfile, long id)
        {
            _geocoderProfileRepository.Save(geocoderProfile, id);
            _uOf.Commit();
        }
    }
}
