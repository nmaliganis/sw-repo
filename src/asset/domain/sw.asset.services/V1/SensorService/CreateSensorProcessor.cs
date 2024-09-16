using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.common.infrastructure.Exceptions.Sensors;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorProcessors;
using sw.asset.model.Devices;
using sw.asset.model.Sensors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.SensorService;
public class CreateSensorProcessor :
    ICreateSensorProcessor
{
    private readonly IUnitOfWork _uOf;
    private readonly ISensorRepository _sensorRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorTypeRepository _sensorTypeRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateSensorProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ISensorRepository sensorRepository, IDeviceRepository deviceRepository, ISensorTypeRepository sensorTypeRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _sensorRepository = sensorRepository;
        _deviceRepository = deviceRepository;
        _sensorTypeRepository = sensorTypeRepository;
    }

    public async Task<BusinessResult<SensorUiModel>> CreateSensorAsync(CreateSensorCommand createCommand)
    {
        var bc = new BusinessResult<SensorUiModel>(new SensorUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var sensorToBeCreated = new Sensor();

            sensorToBeCreated.InjectWithInitialAttributes(
                createCommand.Parameters.Name, 
                createCommand.Parameters.Params,
                createCommand.Parameters.CodeErp, 
                createCommand.Parameters.IsActive, 
                createCommand.Parameters.LastReceivedDate, 
                createCommand.Parameters.LastRecordedDate);
            sensorToBeCreated.InjectWithAudit(createCommand.CreatedById);

            var deviceToBeInjected = _deviceRepository.FindBy(createCommand.Parameters.DeviceId);

            if (deviceToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_DEVICE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }
            var sensorTypesToBeInjected = _sensorTypeRepository.FindAll();

            if (sensorTypesToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_SENSOR_TYPE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            foreach (var sensorType in sensorTypesToBeInjected)
            {
                sensorToBeCreated.InjectWithDevice(deviceToBeInjected);
                sensorToBeCreated.InjectWithSensorType(sensorType);

                this.ThrowExcIfSensorCannotBeCreated(sensorToBeCreated);
                this.ThrowExcIfThisSensorAlreadyExist(sensorToBeCreated);

                Log.Information(
                  $"Create Sensor: {createCommand.Parameters.Name}" +
                  "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
                  "Message: Just Before MakeItPersistence");

                MakeSensorPersistent(sensorToBeCreated);

                Log.Information(
                  $"Create Sensor: {createCommand.Parameters.Name}" +
                  "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
                  "Message: Just After MakeItPersistence");
            }

            bc.Model = ThrowExcIfSensorWasNotBeMadePersistent(sensorToBeCreated);
            bc.Model.Message = "SUCCESS_SENSOR_CREATION";
        }
        catch (InvalidSensorException e)
        {
            string errorMessage = "ERROR_INVALID_SENSOR_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (SensorAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_SENSOR_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensor--  @fail@ [CreateSensorProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (SensorDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_SENSOR_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensor--  @fail@ [CreateSensorProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              $"--CreateSensor--  @fail@ [CreateSensorProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<SensorUiModel>> CreateSensorByImeiAsync(CreateSensorByDeviceImeiCommand createCommand)
    {
        var bc = new BusinessResult<SensorUiModel>(new SensorUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var deviceToBeInjected = _deviceRepository.FindOneByImei(createCommand.deviceImei);

            if (deviceToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_DEVICE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }
            var sensorTypesToBeInjected = _sensorTypeRepository.FindAll();

            if (sensorTypesToBeInjected.IsNull() || sensorTypesToBeInjected.Count <= 0)
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_SENSOR_TYPE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            foreach (var sensorType in sensorTypesToBeInjected)
            {
                var sensorToBeCreated = new Sensor();

                sensorToBeCreated.InjectWithDevice(deviceToBeInjected);
                sensorToBeCreated.InjectWithSensorType(sensorType);
                sensorToBeCreated.InjectWithInitialAttributes(
                    createCommand.Parameters.Name,
                    createCommand.Parameters.Params,
                    createCommand.Parameters.CodeErp,
                    createCommand.Parameters.IsActive,
                    createCommand.Parameters.LastReceivedDate,
                    createCommand.Parameters.LastRecordedDate);
                sensorToBeCreated.InjectWithAudit(createCommand.CreatedById);

                this.ThrowExcIfSensorCannotBeCreated(sensorToBeCreated);

                Log.Information(
                  $"Create Sensor: {createCommand.Parameters.Name + createCommand.Parameters.DeviceImei}" +
                  "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
                  "Message: Just Before MakeItPersistence");

                MakeSensorPersistent(sensorToBeCreated);

                Log.Information(
                  $"Create Sensor: {createCommand.Parameters.Name + createCommand.Parameters.DeviceImei}" +
                  "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
                  "Message: Just After MakeItPersistence");
            }

            bc.Model.Message = "SUCCESS_SENSORS_CREATION";
        }
        catch (InvalidSensorException e)
        {
            string errorMessage = "ERROR_INVALID_SENSOR_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name + createCommand.Parameters.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (SensorAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_SENSOR_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name + createCommand.Parameters.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensor--  @fail@ [CreateSensorProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (SensorDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_SENSOR_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name + createCommand.Parameters.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensor--  @fail@ [CreateSensorProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Sensor: {createCommand.Parameters.Name + createCommand.Parameters.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              $"--CreateSensor--  @fail@ [CreateSensorProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisSensorAlreadyExist(Sensor sensorToBeCreated)
    {
        var sensorRetrieved = this._sensorRepository.FindOneByNameAndDeviceId(sensorToBeCreated.Name, sensorToBeCreated.Device.Id);
        if (sensorRetrieved != null)
        {
            throw new SensorAlreadyExistsException(sensorToBeCreated.Name + sensorToBeCreated.Device.Imei,
              sensorToBeCreated.GetBrokenRulesAsString());
        }
    }

    private SensorUiModel ThrowExcIfSensorWasNotBeMadePersistent(Sensor sensorToBeCreated)
    {
        var retrievedSensor = this._sensorRepository.FindBy(sensorToBeCreated.Id);
        if (retrievedSensor != null)
        {
            return this._autoMapper.Map<SensorUiModel>(retrievedSensor);
        }

        throw new SensorDoesNotExistAfterMadePersistentException(sensorToBeCreated.Name + sensorToBeCreated.Device.Imei);
    }

    private void ThrowExcIfSensorCannotBeCreated(Sensor sensorToBeCreated)
    {
        bool canBeCreated = !sensorToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidSensorException(sensorToBeCreated.GetBrokenRulesAsString());
        }
    }

    private void MakeDevicePersistent(Device device)
    {
        _deviceRepository.Save(device);
        _uOf.Commit();
    }

    private void MakeSensorPersistent(Sensor sensor)
    {
        _sensorRepository.Save(sensor);
        _uOf.Commit();
    }
}
