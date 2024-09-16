using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.common.infrastructure.Exceptions.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Container = sw.asset.model.Assets.Containers.Container;

namespace sw.asset.services.V1.AssetServices.ContainerService;

public class UpdateContainerProcessor : IUpdateContainerProcessor
{
    private readonly IUnitOfWork _uOf;
    private readonly IContainerRepository _containerRepository;
    private readonly IAutoMapper _autoMapper;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorRepository _sensorRepository;
    private readonly IZoneRepository _zoneRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IAssetCategoryRepository _assetCategoryRepository;

    public UpdateContainerProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
      IContainerRepository containerRepository, 
      IDeviceRepository deviceRepository,
      ISensorRepository sensorRepository, 
      IZoneRepository zoneRepository,
      ICompanyRepository companyRepository)
    {
        this._uOf = uOf;
        this._autoMapper = autoMapper;
        this._containerRepository = containerRepository;
        this._deviceRepository = deviceRepository;
        this._sensorRepository = sensorRepository;
        this._sensorRepository = sensorRepository;
        this._zoneRepository = zoneRepository;
        this._companyRepository = companyRepository;
    }

    public async Task<BusinessResult<ContainerUiModel>> UpdateContainerAsync(UpdateContainerCommand updateCommand)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand), "ERROR_UPDATE_CONTAINER_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var containerToBeUpdated = _containerRepository.FindBy(updateCommand.Id);

            if (containerToBeUpdated.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "ERROR_UPDATE_CONTAINER_MODEL"));
                return bc;
            }

            containerToBeUpdated.InjectWithInitialAttributes(
                    updateCommand.Parameters.Name,
                    updateCommand.Parameters.Description,
                    updateCommand.Parameters.CodeErp, 
                    updateCommand.Parameters.Image)
                ;
            
            containerToBeUpdated.InjectWithInitialContainerAttributes(
                    updateCommand.Parameters.Material,
                    updateCommand.Parameters.Capacity,
                    updateCommand.Parameters.WasteType)
                ;
            
            containerToBeUpdated.InjectWithMandatoryPickupContainerAttributes(
                updateCommand.Parameters.MandatoryPickupActive,
                    updateCommand.Parameters.MandatoryPickupDate)
                ;
            
            containerToBeUpdated.InjectWithModifiedAudit(updateCommand.ModifiedById);

            var companyToBeInjected = _companyRepository.FindBy(updateCommand.Parameters.CompanyId);
            if (companyToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_COMPANY_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }
            containerToBeUpdated.InjectWithCompany(companyToBeInjected);
            
            var zoneToBeInjected = _zoneRepository.FindBy(updateCommand.Parameters.ZoneId);
            if (zoneToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_ZONE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }
            containerToBeUpdated.InjectWithZone(zoneToBeInjected);

            var assetCategoryToBeInjected = _assetCategoryRepository.FindBy(updateCommand.Parameters.AssetCategoryId);
            if (assetCategoryToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_ASSET_CATEGORY_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            containerToBeUpdated.InjectWithAssetCategory(assetCategoryToBeInjected);
            
            Log.Information(
              $"Update Container: {updateCommand.Id}" +
              "--UpdateContainerWithMeasurementsAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeContainerPersistent(containerToBeUpdated);

            Log.Information(
              $"Update Container: {updateCommand.Id}" +
              "--UpdateContainerWithMeasurementsAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just After MakeItPersistence");
            
            
            try
            {
                Log.Information(
                    $"Update Container: {containerToBeUpdated.Name}" +
                    "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
                    "Message: Just Before UpdateContainerWithNewPositionById");

                await _containerRepository.UpdateContainerWithNewPositionById(containerToBeUpdated.Id, updateCommand.Parameters.Latitude, updateCommand.Parameters.Longitude);

                Log.Information(
                    $"Update Container: {containerToBeUpdated.Name}" +
                    "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
                    "Message: Just After UpdateContainerWithNewPositionById");
            }
            catch (Exception e)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(containerToBeUpdated.Id), "UpdateContainerWithNewPositionById"));
                Log.Error(
                    $"Update Container: {containerToBeUpdated.Name}" +
                    $"UpdateContainerWithNewPositionById " +
                    $"Exception message: {e.Message} --UpdateContainerWithLatLotAsync--  @NotComplete@ [CreateContainerProcessor].");
                return bc;
            }
            
            bc.Model = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeUpdated);
            bc.Model.Message = "SUCCESS_CONTAINER_UPDATE";
        }
        catch (InvalidContainerException e)
        {
            string errorMessage = "ERROR_INVALID_CONTAINER_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.Id}" +
              $"Error Message:{errorMessage}" +
              "--UpdateContainerWithMeasurementsAsync--  @NotComplete@ [UpdateContainerProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.Id}" +
              $"Error Message:{errorMessage}" +
              $"--OnboardingContainerByNameWithDeviceAsync--  @fail@ [UpdateContainerProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }


        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ContainerUiModel>> UpdateContainerWithMeasurementsAsync(UpdateContainerMeasurementsCommand updateCommand)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var sensorToBeFetched = _sensorRepository.FindByTypeAndDeviceImei(updateCommand.Type, updateCommand.DeviceImei);

            if (sensorToBeFetched.IsNull() || sensorToBeFetched.Count <= 0)
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_FETCH_SENSOR"));
                return bc;
            }

            var containerToBeUpdated = _containerRepository.FindBy(sensorToBeFetched.FirstOrDefault()!.Asset.Id);

            if (containerToBeUpdated.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_FETCH_CONTAINER"));
                return bc;
            }

            containerToBeUpdated.InjectWithModifiedAudit(0);
            if (updateCommand.Type.Contains("Source_Πληρότητα"))
            {
                containerToBeUpdated.CalculateLevel(updateCommand.Parameters.Range);
            }

            Log.Information(
              $"Update Container: {updateCommand.DeviceImei}" +
              "--UpdateContainerWithMeasurementsAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeContainerPersistent(containerToBeUpdated);
            if (updateCommand.Type.Contains("Αποκομιδή"))
            {
                try
                {
                    await _containerRepository.UpdateContainerWithNewPositionById(containerToBeUpdated.Id,
                      updateCommand.Parameters.Latitude, updateCommand.Parameters.Longitude);
                }
                catch (Exception e)
                {
                    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(containerToBeUpdated.Id),
                      "UpdateContainerWithNewPositionById"));
                    Log.Error(
                      $"Update Container: {containerToBeUpdated.Name}" +
                      $"UpdateContainerWithNewPositionById " +
                      $"Exception message: {e.Message} --UpdateContainerWithLatLotAsync--  @NotComplete@ [UpdateContainerProcessor].");
                    return bc;
                }
            }

            Log.Information(
              $"Update Container: {updateCommand.DeviceImei}" +
              "--UpdateContainerWithMeasurementsAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeUpdated);
            bc.Model.Message = "SUCCESS_CONTAINER_UPDATE";
        }
        catch (InvalidContainerException e)
        {
            string errorMessage = "ERROR_INVALID_CONTAINER_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              "--UpdateContainerWithMeasurementsAsync--  @NotComplete@ [UpdateContainerProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              $"--OnboardingContainerByNameWithDeviceAsync--  @fail@ [UpdateContainerProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ContainerUiModel>> UpdateContainerWithMeasurementsForUltrasonicAsync(UpdateContainerMeasurementsCommand updateCommand)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }
        try
        {
            var sensorToBeFetched = _sensorRepository.FindByDeviceImei(updateCommand.DeviceImei);

            if (sensorToBeFetched.IsNull() || sensorToBeFetched.Count <= 0)
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_FETCH_SENSOR"));
                return bc;
            }

            Container containerToBeUpdated = _containerRepository.FindBy(sensorToBeFetched.FirstOrDefault()!.Asset.Id);

            if (containerToBeUpdated.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_FETCH_CONTAINER"));
                return bc;
            }

            containerToBeUpdated.InjectWithModifiedAudit(0);

            containerToBeUpdated.CalculateLevel(updateCommand.Parameters.Range);

            Log.Information(
              $"Update Container: {updateCommand.DeviceImei}" +
              "--UpdateContainerWithMeasurementsAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeContainerPersistent(containerToBeUpdated);

            Log.Information(
              $"Update Container: {updateCommand.DeviceImei}" +
              "--UpdateContainerWithMeasurementsAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeUpdated);
            bc.Model.Message = "SUCCESS_CONTAINER_UPDATE";
        }
        catch (InvalidContainerException e)
        {
            string errorMessage = "ERROR_INVALID_CONTAINER_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              "--UpdateContainerWithMeasurementsAsync--  @NotComplete@ [UpdateContainerProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              $"--OnboardingContainerByNameWithDeviceAsync--  @fail@ [UpdateContainerProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ContainerUiModel>> UpdateContainerWithMeasurementsForMotionAsync(UpdateContainerMeasurementsCommand updateCommand)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }
        try
        {
            var sensorToBeFetched = _sensorRepository.FindByDeviceImei(updateCommand.DeviceImei);

            if (sensorToBeFetched.IsNull() || sensorToBeFetched.Count <= 0)
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_FETCH_SENSOR"));
                return bc;
            }

            Container containerToBeUpdated = _containerRepository.FindBy(sensorToBeFetched.FirstOrDefault()!.Asset.Id);

            if (containerToBeUpdated.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_FETCH_CONTAINER"));
                return bc;
            }

            containerToBeUpdated.InjectWithModifiedAudit(0);

            try
            {
                await _containerRepository.UpdateContainerWithNewPositionById(containerToBeUpdated.Id,
                  updateCommand.Parameters.Latitude, updateCommand.Parameters.Longitude);
            }
            catch (Exception e)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(containerToBeUpdated.Id),
                  "UpdateContainerWithNewPositionById"));
                Log.Error(
                  $"Update Container: {containerToBeUpdated.Name}" +
                  $"UpdateContainerWithNewPositionById " +
                  $"Exception message: {e.Message} --UpdateContainerWithLatLotAsync--  @NotComplete@ [UpdateContainerProcessor].");
                return bc;
            }

            Log.Information(
              $"Update Container: {updateCommand.DeviceImei}" +
              "--UpdateContainerWithMeasurementsAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just After MakeItPersistence");
            bc.Model = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeUpdated);
            bc.Model.Message = "SUCCESS_CONTAINER_UPDATE";
        }
        catch (InvalidContainerException e)
        {
            string errorMessage = "ERROR_INVALID_CONTAINER_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              "--UpdateContainerWithMeasurementsAsync--  @NotComplete@ [UpdateContainerProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {updateCommand.DeviceImei}" +
              $"Error Message:{errorMessage}" +
              $"--OnboardingContainerByNameWithDeviceAsync--  @fail@ [UpdateContainerProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ContainerModificationUiModel>> OnboardingContainerWithDeviceAsync(long containerId, long deviceId, OnboardingContainerWithDeviceCommand updateCommand)
    {
        var bc = new BusinessResult<ContainerModificationUiModel>(new ContainerModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var container = _containerRepository.FindBy(containerId);
        if (container.IsNull())
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(containerId), "Container Id does not exist"));
            return bc;
        }

        var modifiedContainer = _autoMapper.Map<Container>(updateCommand);
        container.Modified(updateCommand.ModifiedById, modifiedContainer);

        var deviceToBeConnected = _deviceRepository.FindBy(deviceId);
        if (deviceToBeConnected.IsNull())
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deviceId), "Device Id does not exist"));
            //Maybe create the Device?
            return bc;
        }

        Persist(container, updateCommand.ContainerId);

        var response = _autoMapper.Map<ContainerModificationUiModel>(container);
        response.Message = $"Container id: {response.Id} updated successfully";

        bc.Model = response;

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ContainerUiModel>> OnboardingContainerByNameWithDeviceAsync(string containerName, string deviceImei, long modifiedBy)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        if (String.IsNullOrEmpty(containerName))
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATED_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var containerToBeUpdated = _containerRepository.FindOneByName(containerName);

            if (containerToBeUpdated.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_CONTAINER_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            var deviceToBeInjected = _deviceRepository.FindOneByImei(deviceImei);
            if (deviceToBeInjected.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_FETCH_DEVICE_NOT_EXISTS"));
                return await Task.FromResult(bc);
            }

            foreach (var sensor in deviceToBeInjected.Sensors)
            {
                containerToBeUpdated.InjectWithSensor(sensor);
            }

            Log.Information(
              $"Update Container: {containerName}" +
              "--OnboardingContainerByNameWithDeviceAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just Before MakeContainerPersistent");

            MakeContainerPersistent(containerToBeUpdated);

            Log.Information(
              $"Update Container: {containerName}" +
              "--OnboardingContainerByNameWithDeviceAsync--  @Complete@ [UpdateContainerProcessor]. " +
              "Message: Just After MakeContainerPersistent");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
              $"Update Container: {containerName}" +
              $"Error Message:{errorMessage}" +
              $"--OnboardingContainerByNameWithDeviceAsync--  @fail@ [UpdateContainerProcessor]. " +
              $"@innerfault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }


    public async Task<BusinessResult<ContainerModificationUiModel>> UpdateContainerWithLatLotAsync(UpdateContainerWithLatLonCommand updateCommand)
    {
        var bc = new BusinessResult<ContainerModificationUiModel>(new ContainerModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var container = _containerRepository.FindBy(updateCommand.Id);
        if (container.IsNull())
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Container Id does not exist"));
            return bc;
        }

        try
        {
            await _containerRepository.UpdateContainerWithNewPositionById(updateCommand.Id, updateCommand.ModifiedLat, updateCommand.ModifiedLon);
        }
        catch (Exception e)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "UNKNOWN_ERROR"));
            Log.Error(
              $"Update Container: {container.Name}" +
              $"unknown error. " +
              $"Exception message: {e.Message} --UpdateContainerWithLatLotAsync--  @NotComplete@ [UpdateContainerProcessor].");
            return bc;
        }

        var response = _autoMapper.Map<ContainerModificationUiModel>(container);
        response.Message = $"Container id: {response.Id} updated successfully";

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ContainerModificationUiModel>> UpdateContainerWithLatLotAsync(UpdateContainerWithLatLonByDeviceCommand updateCommand)
    {
	    var bc = new BusinessResult<ContainerModificationUiModel>(new ContainerModificationUiModel());

	    if (updateCommand.IsNull())
	    {
		    bc.AddBrokenRule(new BusinessError(null));
	    }

	    var containerId = await _containerRepository.FindOneByDeviceImei(updateCommand.DeviceImei);

	    var container = _containerRepository.FindBy(containerId);

	    if (container.IsNull())
	    {
		    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.DeviceImei), "Container Id does not exist"));
		    return bc;
	    }

	    try
	    {
		    await _containerRepository.UpdateContainerWithNewPositionById(container.Id, updateCommand.ModifiedLat, updateCommand.ModifiedLon);
	    }
	    catch (Exception e)
	    {
		    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.DeviceImei), "UNKNOWN_ERROR"));
		    Log.Error(
			    $"Update Container: {container.Id}" +
			    $"unknown error. " +
			    $"Exception message: {e.Message} --UpdateContainerWithLatLotByDeviceAsync--  @NotComplete@ [UpdateContainerProcessor].");
		    return bc;
	    }

	    var response = _autoMapper.Map<ContainerModificationUiModel>(container);
	    response.Message = $"Container id: {response.Id} updated successfully";

	    return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<ContainerModificationUiModel>> UpdateContainerWithLatLotAsync(UpdateContainerByNameWithLatLonCommand updateCommand)
    {
	    var bc = new BusinessResult<ContainerModificationUiModel>(new ContainerModificationUiModel());

	    if (updateCommand.IsNull())
	    {
		    bc.AddBrokenRule(new BusinessError(null));
	    }

	    var container = _containerRepository.FindOneByName(updateCommand.ContainerName);
	    if (container.IsNull())
	    {
		    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.ContainerName), "Container Id does not exist"));
		    return bc;
	    }

	    try
	    {
		    await _containerRepository.UpdateContainerWithNewPositionById(container.Id, updateCommand.ModifiedLat, updateCommand.ModifiedLon);
	    }
	    catch (Exception e)
	    {
		    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.ContainerName), "UNKNOWN_ERROR"));
		    Log.Error(
			    $"Update Container: {container.Name}" +
			    $"unknown error. " +
			    $"Exception message: {e.Message} --UpdateContainerWithLatLotByDeviceAsync--  @NotComplete@ [UpdateContainerProcessor].");
		    return bc;
	    }

	    var modifiedContainer = _containerRepository.FindOneByName(updateCommand.ContainerName);
	    if (modifiedContainer.IsNull())
	    {
		    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.ContainerName), "Container Id does not exist"));
		    return bc;
	    }

		var response = _autoMapper.Map<ContainerModificationUiModel>(modifiedContainer);
	    response.Message = $"Container id: {response.Id} updated successfully";

	    return await Task.FromResult(bc);
    }

	private ContainerUiModel ThrowExcIfContainerWasNotBeMadePersistent(Container containerToHaveBeenUpdated)
    {
        var retrievedContainer = this._containerRepository.FindBy(containerToHaveBeenUpdated.Id);
        if (!retrievedContainer.IsNull())
        {
            return this._autoMapper.Map<ContainerUiModel>(retrievedContainer);
        }

        throw new ContainerDoesNotExistAfterMadePersistentException(containerToHaveBeenUpdated.Name + containerToHaveBeenUpdated.Company.Name);
    }

    private void Persist(Container container, long id)
    {
        _containerRepository.Save(container, id);
        _uOf.Commit();
    }

    private void MakeContainerPersistent(Container container)
    {
        _containerRepository.Save(container);
        _uOf.Commit();
    }
}