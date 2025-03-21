﻿using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.DeviceModelProcessors
{
    public interface IUpdateDeviceModelProcessor
    {
        Task<BusinessResult<DeviceModelModificationUiModel>> UpdateDeviceModelAsync(UpdateDeviceModelCommand updateCommand);
    }
}
