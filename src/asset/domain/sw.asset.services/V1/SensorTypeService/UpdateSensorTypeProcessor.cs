using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.SensorTypes;

namespace sw.asset.services.V1.SensorTypeService
{
    public class UpdateSensorTypeProcessor :
        IUpdateSensorTypeProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ISensorTypeRepository _sensorTypeRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateSensorTypeProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ISensorTypeRepository sensorTypeRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _sensorTypeRepository = sensorTypeRepository;
        }

        public async Task<BusinessResult<SensorTypeModificationUiModel>> UpdateSensorTypeAsync(UpdateSensorTypeCommand updateCommand)
        {
            var bc = new BusinessResult<SensorTypeModificationUiModel>(new SensorTypeModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var sensorType = _sensorTypeRepository.FindBy(updateCommand.Id);
            if (sensorType is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "SensorType Id does not exist"));
                return bc;
            }

            var modifiedSensorType = _autoMapper.Map<SensorType>(updateCommand);
            sensorType.Modified(updateCommand.ModifiedById, modifiedSensorType);

            Persist(sensorType, updateCommand.Id);

            var response = _autoMapper.Map<SensorTypeModificationUiModel>(sensorType);
            response.Message = $"SensorType id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(SensorType sensorType, long id)
        {
            _sensorTypeRepository.Save(sensorType, id);
            _uOf.Commit();
        }
    }
}
