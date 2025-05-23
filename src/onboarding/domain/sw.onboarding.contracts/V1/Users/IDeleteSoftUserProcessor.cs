﻿using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Users
{
    public interface IDeleteSoftUserProcessor
    {
        Task<BusinessResult<UserDeletionUiModel>> DeleteSoftUserAsync(DeleteSoftUserCommand deleteCommand);
    }
}
