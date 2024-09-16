using sw.landmark.common.dtos.V1.Cqrs.Landmarks;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkService
{
    public class CreateLandmarkProcessor :
        ICreateLandmarkProcessor,
        IRequestHandler<CreateLandmarkCommand, BusinessResult<LandmarkCreationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILandmarkRepository _landmarkRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateLandmarkProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ILandmarkRepository landmarkRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _landmarkRepository = landmarkRepository;
        }

        public async Task<BusinessResult<LandmarkCreationUiModel>> Handle(CreateLandmarkCommand createCommand, CancellationToken cancellationToken)
        {
            return await CreateLandmarkAsync(createCommand);
        }

        public async Task<BusinessResult<LandmarkCreationUiModel>> CreateLandmarkAsync(CreateLandmarkCommand createCommand)
        {
            var bc = new BusinessResult<LandmarkCreationUiModel>(new LandmarkCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _landmarkRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "Company name already exists"));
            //}

            var landmark = _autoMapper.Map<Landmark>(createCommand);
            landmark.Created(createCommand.CreatedById);

            Persist(landmark);

            var response = _autoMapper.Map<LandmarkCreationUiModel>(landmark);
            response.Message = "Landmark created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Landmark landmark)
        {
            _landmarkRepository.Add(landmark);
            _uOf.Commit();
        }
    }
}
