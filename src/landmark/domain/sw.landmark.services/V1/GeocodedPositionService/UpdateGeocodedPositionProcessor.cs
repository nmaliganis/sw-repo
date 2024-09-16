using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocodedPositionProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.GeocodedPositionService
{
    public class UpdateGeocodedPositionProcessor :
        IUpdateGeocodedPositionProcessor,
        IRequestHandler<UpdateGeocodedPositionCommand, BusinessResult<GeocodedPositionModificationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocodedPositionRepository _geocodedPositionRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateGeocodedPositionProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IGeocodedPositionRepository geocodedPositionRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _geocodedPositionRepository = geocodedPositionRepository;
        }

        public async Task<BusinessResult<GeocodedPositionModificationUiModel>> Handle(UpdateGeocodedPositionCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateGeocodedPositionAsync(updateCommand);
        }

        public async Task<BusinessResult<GeocodedPositionModificationUiModel>> UpdateGeocodedPositionAsync(UpdateGeocodedPositionCommand updateCommand)
        {
            var bc = new BusinessResult<GeocodedPositionModificationUiModel>(new GeocodedPositionModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var geocodedPosition = _geocodedPositionRepository.FindBy(updateCommand.Id);
            if (geocodedPosition is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "GeocodedPosition Id does not exist"));
                return bc;
            }

            var modifiedGeocodedPosition = _autoMapper.Map<GeocodedPosition>(updateCommand);
            geocodedPosition.Modified(updateCommand.ModifiedById, modifiedGeocodedPosition);

            Persist(geocodedPosition, updateCommand.Id);

            var response = _autoMapper.Map<GeocodedPositionModificationUiModel>(geocodedPosition);
            response.Message = $"GeocodedPosition id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(GeocodedPosition geocodedPosition, long id)
        {
            _geocodedPositionRepository.Save(geocodedPosition, id);
            _uOf.Commit();
        }
    }
}
