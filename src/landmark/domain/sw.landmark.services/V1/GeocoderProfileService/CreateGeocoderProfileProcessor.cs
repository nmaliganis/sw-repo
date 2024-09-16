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
    public class CreateGeocoderProfileProcessor :
        ICreateGeocoderProfileProcessor,
        IRequestHandler<CreateGeocoderProfileCommand, BusinessResult<GeocoderProfileCreationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IGeocoderProfileRepository _geocoderProfileRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateGeocoderProfileProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IGeocoderProfileRepository geocoderProfileRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _geocoderProfileRepository = geocoderProfileRepository;
        }

        public async Task<BusinessResult<GeocoderProfileCreationUiModel>> Handle(CreateGeocoderProfileCommand createCommand, CancellationToken cancellationToken)
        {
            return await CreateGeocoderProfileAsync(createCommand);
        }

        public async Task<BusinessResult<GeocoderProfileCreationUiModel>> CreateGeocoderProfileAsync(CreateGeocoderProfileCommand createCommand)
        {
            var bc = new BusinessResult<GeocoderProfileCreationUiModel>(new GeocoderProfileCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _geocoderProfileRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "Company name already exists"));
            //}

            var geocoderProfile = _autoMapper.Map<GeocoderProfile>(createCommand);
            geocoderProfile.Created(createCommand.CreatedById);

            Persist(geocoderProfile);

            var response = _autoMapper.Map<GeocoderProfileCreationUiModel>(geocoderProfile);
            response.Message = "GeocoderProfile created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(GeocoderProfile geocoderProfile)
        {
            _geocoderProfileRepository.Add(geocoderProfile);
            _uOf.Commit();
        }
    }
}
