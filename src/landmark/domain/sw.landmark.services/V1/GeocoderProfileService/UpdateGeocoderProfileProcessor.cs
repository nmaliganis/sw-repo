using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocoderProfileProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.GeocoderProfileService
{
    public class UpdateGeocoderProfileProcessor :
        IUpdateGeocoderProfileProcessor,
        IRequestHandler<UpdateGeocoderProfileCommand, BusinessResult<GeocoderProfileModificationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocoderProfileRepository _geocoderProfileRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateGeocoderProfileProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IGeocoderProfileRepository geocoderProfileRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _geocoderProfileRepository = geocoderProfileRepository;
        }

        public async Task<BusinessResult<GeocoderProfileModificationUiModel>> Handle(UpdateGeocoderProfileCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateGeocoderProfileAsync(updateCommand);
        }

        public async Task<BusinessResult<GeocoderProfileModificationUiModel>> UpdateGeocoderProfileAsync(UpdateGeocoderProfileCommand updateCommand)
        {
            var bc = new BusinessResult<GeocoderProfileModificationUiModel>(new GeocoderProfileModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var geocoderProfile = _geocoderProfileRepository.FindBy(updateCommand.Id);
            if (geocoderProfile is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "GeocoderProfile Id does not exist"));
                return bc;
            }

            var modifiedGeocoderProfile = _autoMapper.Map<GeocoderProfile>(updateCommand);
            geocoderProfile.Modified(updateCommand.ModifiedById, modifiedGeocoderProfile);

            Persist(geocoderProfile, updateCommand.Id);

            var response = _autoMapper.Map<GeocoderProfileModificationUiModel>(geocoderProfile);
            response.Message = $"GeocoderProfile id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(GeocoderProfile geocoderProfile, long id)
        {
            _geocoderProfileRepository.Save(geocoderProfile, id);
            _uOf.Commit();
        }
    }
}
