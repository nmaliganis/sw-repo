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
    public class UpdateLandmarkProcessor :
        IUpdateLandmarkProcessor,
        IRequestHandler<UpdateLandmarkCommand, BusinessResult<LandmarkModificationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILandmarkRepository _landmarkRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateLandmarkProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ILandmarkRepository landmarkRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _landmarkRepository = landmarkRepository;
        }

        public async Task<BusinessResult<LandmarkModificationUiModel>> Handle(UpdateLandmarkCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateLandmarkAsync(updateCommand);
        }

        public async Task<BusinessResult<LandmarkModificationUiModel>> UpdateLandmarkAsync(UpdateLandmarkCommand updateCommand)
        {
            var bc = new BusinessResult<LandmarkModificationUiModel>(new LandmarkModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var landmark = _landmarkRepository.FindBy(updateCommand.Id);
            if (landmark is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Landmark Id does not exist"));
                return bc;
            }

            var modifiedLandmark = _autoMapper.Map<Landmark>(updateCommand);
            landmark.Modified(updateCommand.ModifiedById, modifiedLandmark);

            Persist(landmark, updateCommand.Id);

            var response = _autoMapper.Map<LandmarkModificationUiModel>(landmark);
            response.Message = $"Landmark id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Landmark landmark, long id)
        {
            _landmarkRepository.Save(landmark, id);
            _uOf.Commit();
        }
    }
}
