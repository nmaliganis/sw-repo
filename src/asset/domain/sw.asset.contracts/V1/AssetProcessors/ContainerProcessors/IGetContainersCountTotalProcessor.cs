﻿using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;

public interface IGetContainersCountTotalProcessor
{
    Task<BusinessResult<ContainerCountUiModel>> GetContainersCountTotalAsync(GetContainersCountTotalQuery qry);
}