﻿using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Roles
{
    public interface IDeleteSoftRoleProcessor
    {
        Task<BusinessResult<RoleDeletionUiModel>> DeleteSoftRoleAsync(DeleteSoftRoleCommand deleteCommand);
    }
}
