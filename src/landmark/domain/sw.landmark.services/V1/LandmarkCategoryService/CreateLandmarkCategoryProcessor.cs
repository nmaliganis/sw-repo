using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.LandmarkCategoryProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.LandmarkCategoryService
{
    public class CreateLandmarkCategoryProcessor :
        ICreateLandmarkCategoryProcessor,
        IRequestHandler<CreateLandmarkCategoryCommand, BusinessResult<LandmarkCategoryCreationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILandmarkCategoryRepository _landmarkCategoryRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateLandmarkCategoryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ILandmarkCategoryRepository landmarkCategoryRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _landmarkCategoryRepository = landmarkCategoryRepository;
        }

        public async Task<BusinessResult<LandmarkCategoryCreationUiModel>> Handle(CreateLandmarkCategoryCommand createCommand, CancellationToken cancellationToken)
        {
            return await CreateLandmarkCategoryAsync(createCommand);
        }

        public async Task<BusinessResult<LandmarkCategoryCreationUiModel>> CreateLandmarkCategoryAsync(CreateLandmarkCategoryCommand createCommand)
        {
            var bc = new BusinessResult<LandmarkCategoryCreationUiModel>(new LandmarkCategoryCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _landmarkCategoryRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "Company name already exists"));
            //}

            var landmarkCategory = _autoMapper.Map<LandmarkCategory>(createCommand);
            landmarkCategory.Created(createCommand.CreatedById);

            Persist(landmarkCategory);

            var response = _autoMapper.Map<LandmarkCategoryCreationUiModel>(landmarkCategory);
            response.Message = "LandmarkCategory created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(LandmarkCategory landmarkCategory)
        {
            _landmarkCategoryRepository.Add(landmarkCategory);
            _uOf.Commit();
        }
    }
}
