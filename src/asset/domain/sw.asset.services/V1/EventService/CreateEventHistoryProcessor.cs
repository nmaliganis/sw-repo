using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.common.infrastructure.Exceptions.Sensors;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.EventProcessors;
using sw.asset.model.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.model.Sensors;

namespace sw.asset.services.V1.EventService;

public class CreateEventHistoryProcessor : ICreateEventHistoryProcessor
{
    private readonly IUnitOfWork _uOf;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorRepository _sensorRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateEventHistoryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IEventHistoryRepository eventHistoryRepository, IDeviceRepository  deviceRepository, ISensorRepository sensorRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _eventHistoryRepository = eventHistoryRepository;
        _deviceRepository = deviceRepository;
        _sensorRepository = sensorRepository;
    }

    public async Task<BusinessResult<EventHistoryUiModel>> CreateEventHistoryAsync(string deviceImei, CreateEventHistoryCommand createCommand)
    {
        var bc = new BusinessResult<EventHistoryUiModel>(new EventHistoryUiModel());
        Sensor sensorToBeUpdated;
        EventHistory eventHistoryToBeCreated;
        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            eventHistoryToBeCreated = _autoMapper.Map<EventHistory>(createCommand);

            eventHistoryToBeCreated.InjectWithParams(createCommand.Parameters.EventValueJson);

            var sensorToBeInjected = _sensorRepository.FindByDeviceImeiAndSensorTypeIndex(deviceImei, createCommand.Type);
            if (sensorToBeInjected.IsNull() || sensorToBeInjected.Count <= 0)
            {
                throw new SensorDoesNotExistException(deviceImei);
            }

            sensorToBeUpdated = sensorToBeInjected.FirstOrDefault();

            if (sensorToBeUpdated.IsNull())
            {
	            throw new SensorDoesNotExistException(deviceImei);
			}

			eventHistoryToBeCreated.InjectWithSensor(sensorToBeUpdated);
            Log.Information(
              $"Create EventHistory: {createCommand.Parameters.Recorded} with value : {createCommand.Parameters.EventValue}" +
              "--CreateEventHistory--  @NotComplete@ [CreateEventHistoryProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeEventHistoryPersistent(eventHistoryToBeCreated);

            Log.Information(
              $"Create EventHistory: {createCommand.Parameters.Recorded} with value : {createCommand.Parameters.EventValue}" +
              "--CreateEventHistory--  @NotComplete@ [CreateEventHistoryProcessor]. " +
              "Message: Just After MakeItPersistence");

        }
        catch (SensorDoesNotExistException ex)
        {
            string errorMessage = "SENSOR_NOT_FOUND";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Create EventHistory: For DeviceImei :{deviceImei} at: {createCommand.Parameters.Recorded} with value : {createCommand.Parameters.EventValue}" +
                $"Error Message:{errorMessage}" +
                $"--CreateEventHistory--  @fail@ [CreateEventHistoryProcessor]. " +
                $"@innerfault:{ex.Message} and {ex.InnerException}");
        }
        catch (Exception e)
        {
            string errorMessage = $"UNKNOWN_ERROR-{e.Message}";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create EventHistory: {createCommand.Parameters.Recorded} with value : {createCommand.Parameters.EventValue}" +
              $"Error Message:{errorMessage}" +
              $"--CreateEventHistory--  @fail@ [CreateEventHistoryProcessor]. " +
              $"@innerfault:{e.Message} and {e.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void MakeEventHistoryPersistent(EventHistory eventHistory)
    {
        this._eventHistoryRepository.Save(eventHistory);
        this._uOf.Commit();
    }

    private void MakeSensorPersistent(Sensor sensor)
    {
	    this._sensorRepository.Save(sensor);
	    this._uOf.Commit();
    }

}//Class : CreateEventHistoryProcessor