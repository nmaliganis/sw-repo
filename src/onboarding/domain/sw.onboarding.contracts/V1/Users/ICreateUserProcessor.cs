﻿using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Users;
using sw.common.dtos.Vms.Accounts;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Users
{
    public interface ICreateUserProcessor
    {
        Task<BusinessResult<UserUiModel>> CreateUserAsync(long accountIdToCreateThisUser, UserForRegistrationUiModel newUserForRegistration);
    }
}
