using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.asset.model.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceModelService
{
    public class CreateDeviceModelProcessor :
        ICreateDeviceModelProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceModelRepository _deviceModelRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateDeviceModelProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDeviceModelRepository deviceModelRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<BusinessResult<DeviceModelCreationUiModel>> CreateDeviceModelAsync(CreateDeviceModelCommand createCommand)
        {
            var bc = new BusinessResult<DeviceModelCreationUiModel>(new DeviceModelCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _deviceModelRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "DeviceModel name already exists"));
            //}

            var deviceModel = new DeviceModel
            {
                Name = createCommand.Name,
                CodeErp = createCommand.CodeErp,
                CodeName = createCommand.CodeName,
                Enabled = createCommand.Enabled
            };
            deviceModel.Created(createCommand.CreatedById);

            Persist(deviceModel);

            var response = _autoMapper.Map<DeviceModelCreationUiModel>(deviceModel);
            response.Message = "DeviceModel created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(DeviceModel deviceModel)
        {
            _deviceModelRepository.Add(deviceModel);
            _uOf.Commit();
        }
    }
}
