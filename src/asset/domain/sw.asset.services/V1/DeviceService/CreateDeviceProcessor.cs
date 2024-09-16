using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.ResourceParameters.Simcards;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.common.infrastructure.Exceptions.Devices;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.asset.model.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceService;

public class CreateDeviceProcessor :
  ICreateDeviceProcessor,
  IRequestHandler<CreateDeviceCommand, BusinessResult<DeviceUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISimcardRepository _simcardRepository;
    private readonly IDeviceModelRepository _deviceModelRepository;
    private readonly ICreateSimcardProcessor _createSimcardProcessor;
    private readonly IAutoMapper _autoMapper;

    public CreateDeviceProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDeviceRepository deviceRepository,
      ISimcardRepository simcardRepository, IDeviceModelRepository deviceModelRepository,
      ICreateSimcardProcessor createSimcardProcessor)
    {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._deviceRepository = deviceRepository;
        this._simcardRepository = simcardRepository;
        this._deviceModelRepository = deviceModelRepository;

        this._createSimcardProcessor = createSimcardProcessor;
    }
    public async Task<BusinessResult<DeviceUiModel>> Handle(CreateDeviceCommand createCommand, CancellationToken cancellationToken)
    {
        return await this.CreateDeviceAsync(createCommand);
    }

    public async Task<BusinessResult<DeviceUiModel>> CreateDeviceAsync(CreateDeviceCommand createCommand)
    {
        var bc = new BusinessResult<DeviceUiModel>(new DeviceUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var deviceToBeCreated = new Device();

            var deviceModelToBeInjected = _deviceModelRepository.FindBy(createCommand.Parameters.DeviceModelId);

            if (deviceModelToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_DEVICE_MODEL_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            var simcardToBeInjected = _simcardRepository.FindOneByNumber(createCommand.Parameters.PhoneNumber);
            if (simcardToBeInjected.IsNull())
            {

                var smartcardHaveBeenCreated = await this._createSimcardProcessor.CreateSimcardAsync(new CreateSimcardCommand(createCommand.CreatedById,
                  new CreateSimcardResourceParameters()
                  {
                      Number = createCommand.Parameters.PhoneNumber,
                      CodeErp = createCommand.Parameters.PhoneNumber
                  }));

                if (smartcardHaveBeenCreated.IsNull() || !smartcardHaveBeenCreated.IsSuccess())
                {
                    bc.Model = null;
                    bc.AddBrokenRule(new BusinessError("ERROR_CREATE_SIM_CARD_NOT_EXISTS"));
                    return await Task.FromResult(bc);
                }

                simcardToBeInjected = _simcardRepository.FindBy(smartcardHaveBeenCreated.Model.Id);
            }

            deviceToBeCreated.InjectWithDeviceModel(deviceModelToBeInjected);
            deviceToBeCreated.InjectWithSimcard(simcardToBeInjected);

            deviceToBeCreated.InjectWithInitialAttributes(
                    createCommand.Parameters.Imei,
                    createCommand.Parameters.SerialNumber,
                    createCommand.Parameters.IpAddress)
                ;

            deviceToBeCreated.InjectWithAudit(createCommand.CreatedById);

            this.ThrowExcIfDeviceCannotBeCreated(deviceToBeCreated);
            this.ThrowExcIfThisDeviceAlreadyExist(deviceToBeCreated);

            Log.Information(
              $"Create Device: {createCommand.Parameters.Imei}" +
              "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeDevicePersistent(deviceToBeCreated);

            Log.Information(
              $"Create Device: {createCommand.Parameters.Imei}" +
              "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfDeviceWasNotBeMadePersistent(deviceToBeCreated);
            bc.Model.Message = "SUCCESS_DEVICE_CREATION";
        }
        catch (InvalidDeviceException e)
        {
            string errorMessage = "ERROR_INVALID_DEVICE_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Device: {createCommand.Parameters.Imei}" +
              $"Error Message:{errorMessage}" +
              "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (DeviceAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_DEVICE_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Device: {createCommand.Parameters.Imei}" +
              $"Error Message:{errorMessage}" +
              "--CreateDevice--  @fail@ [CreateDeviceProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (DeviceDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_DEVICE_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Device: {createCommand.Parameters.Imei}" +
              $"Error Message:{errorMessage}" +
              "--CreateDevice--  @fail@ [CreateDeviceProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Device: {createCommand.Parameters.Imei}" +
              $"Error Message:{errorMessage}" +
              $"--CreateDevice--  @fail@ [CreateDeviceProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }


    private void ThrowExcIfThisDeviceAlreadyExist(Device deviceToBeCreated)
    {
        var deviceRetrieved = this._deviceRepository.FindOneByImei(deviceToBeCreated.Imei);
        if (deviceRetrieved != null)
        {
            throw new DeviceAlreadyExistsException(deviceToBeCreated.Imei,
              deviceToBeCreated.GetBrokenRulesAsString());
        }
    }

    private DeviceUiModel ThrowExcIfDeviceWasNotBeMadePersistent(Device deviceToBeCreated)
    {
        var retrievedDevice = this._deviceRepository.FindOneByImei(deviceToBeCreated.Imei);
        if (retrievedDevice != null)
        {
            return this._autoMapper.Map<DeviceUiModel>(retrievedDevice);
        }

        throw new DeviceDoesNotExistAfterMadePersistentException(deviceToBeCreated.Imei);
    }

    private void ThrowExcIfDeviceCannotBeCreated(Device deviceToBeCreated)
    {
        bool canBeCreated = !deviceToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidDeviceException(deviceToBeCreated.GetBrokenRulesAsString());
        }
    }

    private void MakeDevicePersistent(Device device)
    {
        this._deviceRepository.Save(device);
        this._uOf.Commit();
    }
}