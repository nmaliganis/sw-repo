﻿using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.SensorTypeProcessors;

public interface ICreateSensorTypeProcessor
{
  Task<BusinessResult<SensorTypeUiModel>> CreateSensorTypeAsync(CreateSensorTypeCommand createCommand);
}