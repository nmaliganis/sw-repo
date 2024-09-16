using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.common.infrastructure.Exceptions.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.asset.model.Assets;
using sw.asset.model.Assets.Containers;
using sw.asset.model.Sensors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.ContainerService;

public class CreateContainerProcessor :
  ICreateContainerProcessor
{
    private readonly IUnitOfWork _uOf;
    private readonly IContainerRepository _containerRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ISensorTypeRepository _sensorTypeRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateContainerProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IContainerRepository containerRepository,
      ICompanyRepository companyRepository, IAssetCategoryRepository assetCategoryRepository, IDeviceRepository deviceRepository, ISensorTypeRepository sensorTypeRepository)
    {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._containerRepository = containerRepository;
        this._companyRepository = companyRepository;
        this._assetCategoryRepository = assetCategoryRepository;
        this._deviceRepository = deviceRepository;
        this._sensorTypeRepository = sensorTypeRepository;
    }

    public async Task<BusinessResult<ContainerUiModel>> CreateContainerAsync(CreateContainerCommand createCommand)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }
        try
        {
            Asset containerToBeCreated = new Container();
            containerToBeCreated.InjectWithInitialAttributes(
                createCommand.Parameters.Name,
                createCommand.Parameters.Description,
                createCommand.Parameters.CodeErp, 
                createCommand.Parameters.Image)
                ;

            containerToBeCreated.InjectWithAudit(createCommand.CreatedById);

            var companyToBeInjected = _companyRepository.FindBy(createCommand.Parameters.CompanyId);
            if (companyToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_COMPANY_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }
            containerToBeCreated.InjectWithCompany(companyToBeInjected);

            var assetCategoryToBeInjected = _assetCategoryRepository.FindBy(createCommand.Parameters.AssetCategoryId);
            if (assetCategoryToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_ASSET_CATEGORY_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            containerToBeCreated.InjectWithAssetCategory(assetCategoryToBeInjected);

            this.ThrowExcIfContainerCannotBeCreated((Container)containerToBeCreated);
            this.ThrowExcIfThisContainerAlreadyExist((Container)containerToBeCreated);

            Log.Information(
              $"Create Container: {containerToBeCreated.Name}" +
              "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeContainerPersistent((Container)containerToBeCreated);

            Log.Information(
              $"Create Container: {containerToBeCreated.Name}" +
              "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
              "Message: Just After MakeItPersistence");

            try
            {
                Log.Information(
                  $"Create Container: {containerToBeCreated.Name}" +
                  "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
                  "Message: Just Before UpdateContainerWithNewPositionById");

                await _containerRepository.UpdateContainerWithNewPositionById(containerToBeCreated.Id, createCommand.Parameters.Latitude, createCommand.Parameters.Longitude);

                Log.Information(
                  $"Create Container: {containerToBeCreated.Name}" +
                  "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
                  "Message: Just After UpdateContainerWithNewPositionById");
            }
            catch (Exception e)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(containerToBeCreated.Id), "UpdateContainerWithNewPositionById"));
                Log.Error(
                  $"Update Container: {containerToBeCreated.Name}" +
                  $"UpdateContainerWithNewPositionById " +
                  $"Exception message: {e.Message} --UpdateContainerWithLatLotAsync--  @NotComplete@ [CreateContainerProcessor].");
                return bc;
            }

            bc.Model = ThrowExcIfContainerWasNotBeMadePersistent((Container)containerToBeCreated);
            bc.Model.Message = "SUCCESS_CONTAINER_CREATION";
        }
        catch (InvalidContainerException e)
        {
            string errorMessage = "ERROR_INVALID_CONTAINER_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (ContainerAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_CONTAINER_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateContainer--  @fail@ [CreateContainerProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (ContainerDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_CONTAINER_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateContainer--  @fail@ [CreateContainerProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              $"--CreateContainer--  @fail@ [CreateContainerProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }
        return await Task.FromResult(bc);
    }
    public async Task<BusinessResult<ContainerUiModel>> CreateContainerWithDeviceImeiAsync(string deviceImei, CreateContainerWithDeviceImeiCommand createCommand)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        if (createCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }
        try
        {
            var deviceToBeInjected = _deviceRepository.FindOneByImei(deviceImei);
            if (deviceToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_DEVICE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            if (!deviceToBeInjected.Sensors.IsNull() && deviceToBeInjected.Sensors.Count > 0)
            {
                var asset = deviceToBeInjected.Sensors!.FirstOrDefault()!.Asset;
                if (!asset.IsNull())
                {
                    bc.Model = null;
                    bc.AddBrokenRule(new BusinessError("ERROR_DEVICE_ALREADY_INJECTED_WITH_ASSET"));
                    return await Task.FromResult(bc);
                }
            }

            Asset containerToBeCreated = new Container();

            containerToBeCreated.InjectWithInitialAttributes(
                    createCommand.Parameters.Name,
                    createCommand.Parameters.Description,
                    createCommand.Parameters.CodeErp,
                    createCommand.Parameters.Image)
                ;

            containerToBeCreated.InjectWithAudit(createCommand.CreatedById);

            var companyToBeInjected = _companyRepository.FindBy(createCommand.Parameters.CompanyId);
            if (companyToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_COMPANY_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }
            containerToBeCreated.InjectWithCompany(companyToBeInjected);

            var assetCategoryToBeInjected = _assetCategoryRepository.FindBy(createCommand.Parameters.AssetCategoryId);
            if (assetCategoryToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_ASSET_CATEGORY_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }
            containerToBeCreated.InjectWithAssetCategory(assetCategoryToBeInjected);


            var sensorTypesToBeInjected = _sensorTypeRepository.FindAll();

            if (sensorTypesToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_SENSOR_TYPE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            foreach (var sensorType in sensorTypesToBeInjected)
            {
                Sensor sensorToBeCreated = new Sensor();

                sensorToBeCreated.InjectWithDevice(deviceToBeInjected);
                sensorToBeCreated.InjectWithSensorType(sensorType);

                Log.Information(
                    $"Create Sensor: {createCommand.Parameters.Name}" +
                    "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                containerToBeCreated.InjectWithSensor(sensorToBeCreated);

                Log.Information(
                    $"Create Sensor: {createCommand.Parameters.Name}" +
                    "--CreateSensor--  @NotComplete@ [CreateSensorProcessor]. " +
                    "Message: Just After MakeItPersistence");
            }

            containerToBeCreated.InjectWithInitialAttributes(
                    createCommand.Parameters.Name,
                    createCommand.Parameters.Description,
                    createCommand.Parameters.CodeErp,
                    createCommand.Parameters.Image)
                ;
            containerToBeCreated.InjectWithAudit(createCommand.CreatedById);

            this.ThrowExcIfContainerCannotBeCreated((Container)containerToBeCreated);
            this.ThrowExcIfThisContainerAlreadyExist((Container)containerToBeCreated);

            Log.Information(
              $"Create Container: {containerToBeCreated.Name}" +
              "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeContainerPersistent((Container)containerToBeCreated);

            Log.Information(
              $"Create Container: {containerToBeCreated.Name}" +
              "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
              "Message: Just After MakeItPersistence");

            bc.Model = ThrowExcIfContainerWasNotBeMadePersistent((Container)containerToBeCreated);
            bc.Model.Message = "SUCCESS_CONTAINER_CREATION";
        }
        catch (InvalidContainerException e)
        {
            string errorMessage = "ERROR_INVALID_CONTAINER_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (ContainerAlreadyExistsException ex)
        {
            string errorMessage = "ERROR_CONTAINER_ALREADY_EXISTS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateContainer--  @fail@ [CreateContainerProcessor]. " +
              $"@innerfault:{ex?.Message} and {ex?.InnerException}");
        }
        catch (ContainerDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_CONTAINER_NOT_MADE_PERSISTENT";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              "--CreateContainer--  @fail@ [CreateContainerProcessor]." +
              $" @innerfault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Create Container: {createCommand.Parameters.Name}" +
              $"Error Message:{errorMessage}" +
              $"--CreateContainer--  @fail@ [CreateContainerProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }
        return await Task.FromResult(bc);
    }

    private void ThrowExcIfThisContainerAlreadyExist(Container containerToBeCreated)
    {
        var containerRetrieved = this._containerRepository.FindOneByNameAndCompanyId(containerToBeCreated.Name, containerToBeCreated.Company.Id);
        if (!containerRetrieved.IsNull())
        {
            throw new ContainerAlreadyExistsException(containerToBeCreated.Name + containerToBeCreated.Company.Name,
              containerToBeCreated.GetBrokenRulesAsString());
        }
    }

    private ContainerUiModel ThrowExcIfContainerWasNotBeMadePersistent(Container containerToBeCreated)
    {
        var retrievedContainer = this._containerRepository.FindBy(containerToBeCreated.Id);
        if (!retrievedContainer.IsNull())
        {
            return this._autoMapper.Map<ContainerUiModel>(retrievedContainer);
        }

        throw new ContainerDoesNotExistAfterMadePersistentException(containerToBeCreated.Name + containerToBeCreated.Company.Name);
    }

    private void ThrowExcIfContainerCannotBeCreated(Container containerToBeCreated)
    {
        bool canBeCreated = !containerToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
        {
            throw new InvalidContainerException(containerToBeCreated.GetBrokenRulesAsString());
        }
    }

    private void MakeContainerPersistent(Container container)
    {
        _containerRepository.Save(container);
        _uOf.Commit();
    }
}// Class: CreateContainerProcessor