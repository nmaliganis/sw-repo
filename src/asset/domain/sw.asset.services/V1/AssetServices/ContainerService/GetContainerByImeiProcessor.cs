using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.asset.model.Assets.Containers;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.model.Assets;

namespace sw.asset.services.V1.AssetServices.ContainerService;

public class GetContainerByImeiProcessor : IGetContainerByImeiProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IContainerRepository _containerRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorRepository _sensorRepository;
    public GetContainerByImeiProcessor(IContainerRepository containerRepository, ISensorRepository sensorRepository, IDeviceRepository deviceRepository, IAutoMapper autoMapper)
    {
        this._containerRepository = containerRepository;
        this._sensorRepository = sensorRepository;
        this._deviceRepository = deviceRepository;

        _autoMapper = autoMapper;
    }

    public async Task<BusinessResult<ContainerUiModel>> GetContainerByImeiAsync(string imei)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        var deviceToBeFetched = this._deviceRepository.FindOneByImei(imei);

        if (deviceToBeFetched.IsNull())
        {
            string errorMessage = "ERROR_INVALID_DEVICE_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Get Container By Imei : {imei}" +
                $"Error Message:{errorMessage}" +
                "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor].");

            return await Task.FromResult(bc);
        }

        if (deviceToBeFetched.Sensors.IsNull() || deviceToBeFetched.Sensors.Count <= 0)
        {
            string errorMessage = "ERROR_INVALID_DEVICE_SENSORS";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Get Container By Imei : {imei}" +
                $"Error Message:{errorMessage}" +
                "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor].");

            return await Task.FromResult(bc);
        }

        Asset assetToBeFetched = deviceToBeFetched.Sensors.FirstOrDefault()!.Asset;

        Container containerToBeFetched = this._containerRepository.FindBy(assetToBeFetched.Id);

        var response = _autoMapper.Map<ContainerUiModel>(containerToBeFetched);

        response.Message = $"Container id: {response.Id} fetched successfully";

        bc.Model = response;

        Log.Information(
            $"Get Container By Imei : {imei}" +
            $"{response.Message}");

        return await Task.FromResult(bc);
    }

}// Class: GetContainerByIdProcessor