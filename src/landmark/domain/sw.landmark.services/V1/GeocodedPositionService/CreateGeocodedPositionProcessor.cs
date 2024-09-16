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
    public class CreateGeocodedPositionProcessor :
        ICreateGeocodedPositionProcessor,
        IRequestHandler<CreateGeocodedPositionCommand, BusinessResult<GeocodedPositionCreationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocodedPositionRepository _geocodedPositionRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateGeocodedPositionProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IGeocodedPositionRepository geocodedPositionRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _geocodedPositionRepository = geocodedPositionRepository;
        }

        public async Task<BusinessResult<GeocodedPositionCreationUiModel>> Handle(CreateGeocodedPositionCommand createCommand, CancellationToken cancellationToken)
        {
            return await CreateGeocodedPositionAsync(createCommand);
        }

        public async Task<BusinessResult<GeocodedPositionCreationUiModel>> CreateGeocodedPositionAsync(CreateGeocodedPositionCommand createCommand)
        {
            var bc = new BusinessResult<GeocodedPositionCreationUiModel>(new GeocodedPositionCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _geocodedPositionRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "Company name already exists"));
            //}

            var geocodedPosition = _autoMapper.Map<GeocodedPosition>(createCommand);
            geocodedPosition.Created(createCommand.CreatedById);

            Persist(geocodedPosition);

            var response = _autoMapper.Map<GeocodedPositionCreationUiModel>(geocodedPosition);
            response.Message = "GeocodedPosition created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(GeocodedPosition geocodedPosition)
        {
            _geocodedPositionRepository.Add(geocodedPosition);
            _uOf.Commit();
        }
    }
}
