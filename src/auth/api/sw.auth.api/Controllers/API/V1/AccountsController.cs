using sw.auth.api.Validators;
using sw.auth.common.dtos.Cqrs.Members;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Accounts;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace sw.auth.api.Controllers.API.V1;

/// <summary>
/// Class : AccountsController
/// </summary>
[ApiVersionNeutral]
[Produces("application/json")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/[controller]")]
[ApiController]
public class AccountsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="urlHelper"></param>
    /// <param name="typeHelperService"></param>
    /// <param name="propertyMappingService"></param>
    public AccountsController(
        IMediator mediator,
        IUrlHelper urlHelper,
        ITypeHelperService typeHelperService,
        IPropertyMappingService propertyMappingService)
    {
        this._mediator = mediator;
        this._urlHelper = urlHelper;
        this._typeHelperService = typeHelperService;
        this._propertyMappingService = propertyMappingService;
    }

    /// <summary>
    /// POST : Register a new user.
    /// </summary>
    /// <param name="managedUserVm">managedUserVM the managed user View Model</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new user is registered or 400 (Bad Request) if the login or email is already in use or Validation Request Model Error </remarks>
    /// <response code="201">Created (if the user is registered)</response>
    /// <response code="400">email in use</response>
    [Route("register", Name = "PostAccountRegisterRoute")]
    [HttpPost]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> PostAccountRegisterAsync([FromBody] UserForRegistrationUiModel managedUserVm)
    {
        var response = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (response.IsNull())
        {
            return this.NotFound("INVALID_AUTH_USER");
        }

        if (!response.IsSuccess())
        {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(response.BrokenRules);
        }

        var responseHasAlreadyThisUser = await this._mediator.Send(new SearchMemberByEmailOrLoginQuery(managedUserVm.Email, managedUserVm.Login));

        if (response.IsNull())
        {
            return this.NotFound("INVALID_SEARCH_USER");
        }

        if (!response.IsSuccess())
        {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(response.BrokenRules);
        }

        if (responseHasAlreadyThisUser.Model)
        {
            Log.Error($"--Method:PostAccountRegisterAsync -- Message:USER_REGISTERED_LOGIN_OR_EMAIL_ALREADY_EXIST-- Datetime:{DateTime.Now} " +
                      $"-- UserInfo: Login: {managedUserVm.Login} Email: {managedUserVm.Email}, ");
            return this.BadRequest(new { errorMessage = "USERNAME_OR_EMAIL_ALREADY_EXISTS" });
        }

        var responseRegisterUser = await this._mediator.Send(new CreateUserCommand(response.Model.Id, managedUserVm));

        if (responseRegisterUser.IsNull())
        {
            return this.NotFound("ERROR_REGISTERING_USER");
        }

        if (!response.IsSuccess())
        {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Created("/register", new
        {
            id = responseRegisterUser.Model.Id,
            username = managedUserVm.Login,
            email = managedUserVm.Email,
            isActivated = false,
            message = "SUCCESS_REGISTERED_USER",
            status = "user created - An activation code was created - Needs Activation"
        });
    }


    /// <summary>
    /// PUT : Activate the registered user.
    /// </summary>
    /// <param name="userIdToBeActivated">registeredUser Registered User Id to be activated</param>
    /// <param name="accountForActivation">Account for Activation</param>
    /// <remarks> return the ResponseEntity with status 200 (OK) and the activated user in body, or status 500 (Internal Server Error) if the user couldn't be activated </remarks>
    /// <response code="200">(OK) and the activated user in body</response>
    /// <response code="500">500 (Internal Server Error)</response>
    [Route("activate/{userIdToBeActivated}", Name = "PutAccountActivateRoute")]
    [HttpPut]
    [Authorize(Roles = "SU, ADMIN")]
    [ValidateModel]
    public async Task<IActionResult> PutAccountActivateAsync(long userIdToBeActivated,
        [FromBody] AccountForActivationModification accountForActivation)
    {
        if (userIdToBeActivated <= 0 || accountForActivation.ActivationCode == Guid.Empty)
        {
            Log.Error(
                $"--Method:PutAccountActivateAsync -- Message:ERROR_VALIDATION_MODEL" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest("ERROR_VALIDATION_MODEL");
        }

        var responseRegisterUser = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (responseRegisterUser.IsNull())
        {
            Log.Error(
                $"--Method:GetUserByEmailQuery -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_AUDIT_USER" });
        }

        var userAudit = responseRegisterUser.Model;

        if (userAudit.IsNull())
        {
            Log.Error(
                $"--Method:PutAccountActivateAsync -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_AUDIT_USER" });
        }

        if (!userAudit.IsActivated)
        {
            Log.Error(
                $"--Method:PutAccountActivateAsync -- Message:USER_ACTION_NOT_ALLOWED" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "USER_ACTION_NOT_ALLOWED" });
        }

        var userHasBeenActivated = await this._mediator.Send(new UpdateUserActivationCommand(userIdToBeActivated, userAudit.Id, accountForActivation));
        Log.Information(
            $"--Method:PutAccountActivateAsync -- Message:USER_ACTIVATED" +
            $" -- {userHasBeenActivated.Model.Id} -- Datetime:{DateTime.UtcNow}");

        var userHasManipulatedWithActivation = await this._mediator.Send(new GetUserByIdQuery(userHasBeenActivated.Model.Id));

        return this.Ok(userHasManipulatedWithActivation.Model);
    }

    /// <summary>
    /// PUT : Activate the registered user.
    /// </summary>
    /// <param name="accountForActivation">Account for Activation</param>
    /// <remarks> return the ResponseEntity with status 200 (OK) and the activated user in body, or status 500 (Internal Server Error) if the user couldn't be activated </remarks>
    /// <response code="200">(OK) and the activated user in body</response>
    /// <response code="500">500 (Internal Server Error)</response>
    [Route("activate", Name = "PutAccountActivateWithActivationKeyRoot")]
    [HttpPut]
    [ValidateModel]
    [AllowAnonymous]
    public async Task<IActionResult> PutAccountActivateWithActivationKeyAsync([FromBody] AccountForActivationModification accountForActivation)
    {
        if (accountForActivation.IsNull() || accountForActivation.ActivationCode == Guid.Empty)
        {
            Log.Error(
                $"--Method:PutAccountActivateWithActivationKeyRoot -- Message:ERROR_VALIDATION_MODEL" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_VALIDATION_MODEL" });
        }

        var userHasBeenActivated = await this._mediator.Send(new UpdateUserOnlyWithActivationCommand(accountForActivation));
        Log.Information(
            $"--Method:PutAccountActivateAsync -- Message:USER_ACTIVATED" +
            $" -- {userHasBeenActivated.Model.Id} -- Datetime:{DateTime.UtcNow}");

        var userHasManipulatedWithActivation = await this._mediator.Send(new GetUserByIdQuery(userHasBeenActivated.Model.Id));

        return this.Ok(userHasManipulatedWithActivation.Model.Id);
    }

    /// <summary>
    /// POST  /account/change_password : changes the current user's password - Change
    /// </summary>
    /// <remarks> return the current user </remarks>
    /// <response code="200">200 (OK) and the updated user in body</response>
    /// <response code="400">400 (Bad Request)</response>
    /// <response code="500">500 (Internal Server Error) if the user couldn't be updated</response>
    [Route("change_password/{userIdToBePasswordChanged}", Name = "PostAccountChangePasswordRoute")]
    [HttpPost]
    [Authorize(Roles = "SU, ADMIN")]
    [ValidateModel]
    public async Task<IActionResult> PostAccountChangePasswordAsync(long userIdToBePasswordChanged,
        [FromBody] ChangePasswordUiModel changePasswordUiModel)
    {
        if (userIdToBePasswordChanged <= 0)
        {
            Log.Error(
                $"--Method:PostAccountChangePasswordAsync -- Message:ERROR_VALIDATION_ID_MODEL" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest("ERROR_VALIDATION_ID_MODEL");
        }

        var responseRegisterUser = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (responseRegisterUser.IsNull())
        {
            Log.Error(
                $"--Method:GetUserByEmailQuery -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_AUDIT_USER" });
        }

        var userAudit = responseRegisterUser.Model;

        if (userAudit.IsNull())
        {
            Log.Error(
                $"--Method:PostAccountChangePasswordAsync -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "USER_ACTION_NOT_EXISTS" });
        }

        var userToHaveBeenUpdateResult = await this._mediator.Send(new UpdateUserWithNewPasswordCommand(
            userAudit.Id, userIdToBePasswordChanged, changePasswordUiModel));

        if (!userToHaveBeenUpdateResult.Model)
        {
            Log.Error(
                $"--Method:PostAccountChangePasswordAsync -- Message:ERROR_PASSWORD_CHANGED" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest("ERROR_PASSWORD_CHANGED");
        }

        Log.Information(
            $"--Method:PostAccountChangePasswordAsync -- Message:ERROR_PASSWORD_CHANGED" +
            $" -- Datetime:{DateTime.UtcNow}");
        return this.Ok("SUCCESS_PASSWORD_CHANGED");
    }

    /// <summary>
    /// POST  /account/change_password : changes the current user's password - Forget
    /// </summary>
    /// <remarks> return the current user </remarks>
    /// <response code="200">200 (OK) and the updated user in body</response>
    /// <response code="400">400 (Bad Request)</response>
    /// <response code="500">500 (Internal Server Error) if the user couldn't be updated</response>
    [Route("members/{memberId}/change-status", Name = "UpdateAccountChangeStatusMemberRoot")]
    [HttpPut]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> UpdateAccountChangeStatusMemberAsync(long memberId,
        [FromBody] ChangeStatusUiModel changedStatus)
    {
        var responseRegisterUser = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (responseRegisterUser.IsNull())
        {
            Log.Error(
                $"--Method:GetUserByEmailQuery -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_AUDIT_USER" });
        }

        var userAudit = responseRegisterUser.Model;

        if (userAudit.IsNull())
        {
            Log.Error(
                $"--Method:UpdateAccountChangeStatusMemberAsync -- Message:ERROR_VALIDATION_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest("ERROR_VALIDATION_AUDIT_USER");
        }

        if (memberId <= 0)
        {
            return this.BadRequest("ERROR_INVALID_MEMBER_ID");
        }

        var changedUser = await this._mediator.Send(new UpdateUserStatusCommand(memberId,
            GetCompanyFromClaims(), userAudit.Id, changedStatus));

        return this.Ok(changedUser.Model);
    }


    /// <summary>
    /// POST  /account/change_password : changes the current user's password - Forget
    /// </summary>
    /// <remarks> return the current user </remarks>
    /// <response code="200">200 (OK) and the updated user in body</response>
    /// <response code="400">400 (Bad Request)</response>
    /// <response code="500">500 (Internal Server Error) if the user couldn't be updated</response>
    [Route("members/{memberId}/logout", Name = "UpdateAccountChangeLoginStatusRoot")]
    [HttpPut]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> UpdateAccountChangeLoginStatusAsync(long memberId)
    {
        var responseRegisterUser = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (responseRegisterUser.IsNull())
        {
            Log.Error(
                $"--Method:GetUserByEmailQuery -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_AUDIT_USER" });
        }

        var userAudit = responseRegisterUser.Model;

        if (userAudit.IsNull())
        {
            Log.Error(
                $"--Method:UpdateAccountChangeLoginStatusAsync -- Message:ERROR_VALIDATION_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest("ERROR_VALIDATION_AUDIT_USER");
        }

        if (memberId <= 0)
        {
            return this.BadRequest("ERROR_INVALID_MEMBER_ID");
        }

        var isLogout = await this._mediator.Send(
            new UpdateUserLogoutCommand(memberId, userAudit.Id));

        return this.Ok(isLogout);
    }


    /// <summary>
    /// POST  PostAccountForgetPasswordInitRoute
    /// </summary>
    /// <remarks> return the current user </remarks>
    /// <response code="200">200 (OK) and the updated user in body</response>
    /// <response code="400">400 (Bad Request)</response>
    /// <response code="500">500 (Internal Server Error) if the user couldn't be updated</response>
    [Route("forget-password/init/{email}", Name = "PostAccountForgetPasswordInitRoute")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> PostAccountForgetPasswordInitAsync(string email)
    {
        Log.Information(
            $"--Method:PostAccountForgetPasswordInitAsync -- Message:INIT-FORGET_PASSWORD" +
            $" -- Datetime:{DateTime.UtcNow}");

        var isReActivated = await this._mediator.Send(new ForgetMemberPasswordByEmailCommand(email));

        return this.Ok(isReActivated);
    }

    /// <summary>
    /// POST  PostAccountForgetPasswordFinishRoute
    /// </summary>
    /// <remarks> return the current user </remarks>
    /// <response code="200">200 (OK) and the updated user in body</response>
    /// <response code="400">400 (Bad Request)</response>
    /// <response code="500">500 (Internal Server Error) if the user couldn't be updated</response>
    [Route("forget-password/finish/{activationCode}", Name = "PostAccountForgetPasswordFinishRoute")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> PostAccountForgetPasswordFinishAsync(Guid activationCode)
    {
        if (activationCode == Guid.Empty)
        {
            Log.Error(
                $"--Method:PostAccountForgetPasswordFinishAsync -- Message:ERROR_VALIDATION_MODEL" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_VALIDATION_MODEL" });
        }

        var userHasBeenActivated = await this._mediator.Send(new UpdateUserOnlyWithActivationDirectCommand(activationCode));
        Log.Information(
            $"--Method:PostAccountForgetPasswordFinishAsync -- Message:USER_ACTIVATED" +
            $" -- {userHasBeenActivated.Model.Id} -- Datetime:{DateTime.UtcNow}");
        return this.Ok(userHasBeenActivated);
    }

    /// <summary>
    /// PUT   /account/reset-password/init : Send an email to reset the password of the user
    /// </summary>
    /// <remarks> return the current user </remarks>
    /// <response code="200">200 (OK) and the updated user in body</response>
    /// <response code="400">400 (Bad Request)</response>
    /// <response code="500">500 (Internal Server Error) if the user couldn't be updated</response>
    [Route("reset-password/init", Name = "PostAccountResetPasswordInitRoute")]
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PostAccountResetPasswordInitAsync(
        [FromBody] ForgetPasswordUiModel forgetPasswordUiModel)
    {
        var responseRegisterUser = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (responseRegisterUser.IsNull())
        {
            Log.Error(
                $"--Method:GetUserByEmailQuery -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_AUDIT_USER" });
        }

        var userAudit = responseRegisterUser.Model;

        return this.Ok();
    }

    /// <summary>
    /// POST   /account/reset-password/finish : Finish to reset the password of the user
    /// </summary>
    /// <remarks> return the current user </remarks>
    /// <response code="200">200 (OK) and the updated user in body</response>
    /// <response code="400">400 (Bad Request)</response>
    /// <response code="500">500 (Internal Server Error) if the user couldn't be updated</response>
    [Route("reset-password/finish", Name = "PostAccountResetPasswordFinishRoute")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostAccountResetPasswordFinishAsync(
        [FromBody] ForgetPasswordUiModel forgetPasswordUiModel)
    {
        var responseRegisterUser = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (responseRegisterUser.IsNull())
        {
            Log.Error(
                $"--Method:GetUserByEmailQuery -- Message:ERROR_AUDIT_USER" +
                $" -- Datetime:{DateTime.UtcNow}");
            return this.BadRequest(new { errorMessage = "ERROR_AUDIT_USER" });
        }

        var userAudit = responseRegisterUser.Model;

        return this.Ok();
    }

}//Class : AccountsController