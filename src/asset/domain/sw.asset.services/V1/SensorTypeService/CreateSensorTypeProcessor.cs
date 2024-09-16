using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.common.infrastructure.Exceptions.SensorTypes;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.asset.model.SensorTypes;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.SensorTypeService;

public class CreateSensorTypeProcessor :
  ICreateSensorTypeProcessor
{
    private readonly IUnitOfWork _uOf;
    private readonly ISensorTypeRepository _sensorTypeRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateSensorTypeProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ISensorTypeRepository sensorTypeRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _sensorTypeRepository = sensorTypeRepository;
    }

    public async Task<BusinessResult<SensorTypeUiModel>> CreateSensorTypeAsync(
      CreateSensorTypeCommand createCommand)
    {
        var bc = new BusinessResult<SensorTypeUiModel>(new SensorTypeUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var sensorTypeToBeCreated = _autoMapper.Map<SensorType>(createCommand);
            sensorTypeToBeCreated.InjectWithAudit(createCommand.CreatedById);

            this.ThrowExcIfSensorTypeCannotBeCreated(sensorTypeToBeCreated);
            this.ThrowExcIfThisSensorTypeAlreadyExist(sensorTypeToBeCreated);

            Log.Information(
              $"Create SensorType: {createCommand.Parameters.Name}" +
              "--CreateSensorType--  @NotComplete@ [CreateSensorTypeProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeSensorTypePersistent(sensorTypeToBeCreated);

            Log.Information(
              $"Create SensorType: {createCommand.Parameters.Name}" +
              "--CreateSensorType--  @NotComplete@ [CreateSensorTypeProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfSensorTypeWasNotBeMadePersistent(sensorTypeToBeCreated);
        }
        catch (InvalidSensorTypeException e)
        {
            string errorMessage = "ERROR_INVALID_SENSOR_TYPE_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create SensorType: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensorType--  @NotComplete@ [CreateSensorTypeProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (SensorTypeAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_SENSOR_TYPE_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create SensorType: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensorType--  @fail@ [CreateSensorTypeProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (SensorTypeDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_SENSOR_TYPE_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create SensorType: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateSensorType--  @fail@ [CreateSensorTypeProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create SensorType: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              $"--CreateSensorType--  @fail@ [CreateSensorTypeProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisSensorTypeAlreadyExist(SensorType sensorTypeToBeCreated)
    {
        var sensorTypeRetrieved = this._sensorTypeRepository.FindSensorTypeByName(sensorTypeToBeCreated.Name);
        if (!sensorTypeRetrieved.IsNull())
        {
            throw new SensorTypeAlreadyExistsException(sensorTypeToBeCreated.Name,
              sensorTypeToBeCreated.GetBrokenRulesAsString());
        }
    }

    private SensorTypeUiModel ThrowExcIfSensorTypeWasNotBeMadePersistent(SensorType sensorTypeToBeCreated)
    {
        var retrievedSensorType = this._sensorTypeRepository.FindSensorTypeByName(sensorTypeToBeCreated.Name);
        if (!retrievedSensorType.IsNull())
        {
            return this._autoMapper.Map<SensorTypeUiModel>(retrievedSensorType);
        }

        throw new SensorTypeDoesNotExistAfterMadePersistentException(sensorTypeToBeCreated.Name);
    }

    private void ThrowExcIfSensorTypeCannotBeCreated(SensorType sensorTypeToBeCreated)
    {
        bool canBeCreated = !sensorTypeToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidSensorTypeException(sensorTypeToBeCreated.GetBrokenRulesAsString());
        }
    }

    private void MakeSensorTypePersistent(SensorType sensorType)
    {
        this._sensorTypeRepository.Save(sensorType);
        this._uOf.Commit();
    }
}//Class : CreateSensorTypeProcessor