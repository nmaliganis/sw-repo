using sw.auth.api.Validators;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.model.Users;
using sw.common.dtos.ResourceParameters.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace sw.auth.api.Controllers.API.V1
{
    /// <summary>
    /// Class : User Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController
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
        public UsersController(
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
        /// Get - Fetch all Users
        /// </summary>
        /// <param name="parameters">User parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all Users </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetUsersAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetUsersAsync(
            [FromQuery] GetUsersResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull())
            {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<UserUiModel, User>(parameters.OrderBy))
            {
                return this.BadRequest("USER_MODEL_ERROR");
            }

            if (!this._typeHelperService.TypeHasProperties<UserUiModel>
                (parameters.Fields))
            {
                return this.BadRequest("USER_FIELDS_ERROR");
            }

            var companyIdFromClaims = this.GetCompanyFromClaims();

            if (companyIdFromClaims <= 0)
            {
                return this.BadRequest("ERROR_INVALID_COMPANY");
            }

            BusinessResult<PagedList<UserUiModel>> fetchedUsers;
            if (this.IsSuFromClaims())
            {
                try
                {
                    Log.Information(
                        $"--Method:GetUsersAsync -- Message:USERS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just Before : GetUsersAsync");
                    fetchedUsers = await this._mediator.Send(new GetUsersQuery(parameters));
                    Log.Information(
                        $"--Method:GetUsersAsync -- Message:USERS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just After : GetUsersAsync");
                }
                catch (Exception e)
                {
                    Log.Error(
                        $"--Method:GetUsersAsync -- Message: USERS_FETCH_ERROR" +
                        $" -- Datetime:{DateTime.Now} -- UserInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                    return this.BadRequest("ERROR_FETCH_UserS");
                }
            }
            else
            {
                try
                {
                    Log.Information(
                        $"--Method:GetUsersAsync -- Message:USERS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just Before : GetUsersAsync");
                    fetchedUsers = await this._mediator.Send(new GetUsersByCompanyQuery(companyIdFromClaims, parameters));
                    Log.Information(
                        $"--Method:GetUsersAsync -- Message:UserS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just After : GetUsersAsync");
                }
                catch (Exception e)
                {
                    Log.Error(
                        $"--Method:GetUsersAsync -- Message:UserS_FETCH_ERROR" +
                        $" -- Datetime:{DateTime.Now} -- UserInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                    return this.BadRequest("ERROR_FETCH_UserS");
                }
            }


            if (fetchedUsers.IsNull())
            {
                return this.NotFound("USER_NOT_FOUND");
            }

            if (!fetchedUsers.IsSuccess())
            {
                return this.OkOrBadRequest(fetchedUsers.BrokenRules);
            }

            if (fetchedUsers.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(fetchedUsers.BrokenRules);
            }

            var responseWithMetaData = this.CreateOkWithMetaData(fetchedUsers.Model, mediaType, parameters, this._urlHelper, "GetUserByIdAsync", "GetUsersAsync");
            return this.Ok(responseWithMetaData);
        }


        /// <summary>
        /// Get - Fetch all Users By Department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="parameters">User parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all Users </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("departments/{departmentId}", Name = "GetUsersByDepartmentAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetUsersByDepartmentAsync(long departmentId,
            [FromQuery] GetUsersResourceParameters parameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull())
            {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<UserUiModel, User>(parameters.OrderBy))
            {
                return this.BadRequest("USER_MODEL_ERROR");
            }

            if (!this._typeHelperService.TypeHasProperties<UserUiModel>
                (parameters.Fields))
            {
                return this.BadRequest("USER_FIELDS_ERROR");
            }

            var companyIdFromClaims = this.GetCompanyFromClaims();

            if (companyIdFromClaims <= 0)
            {
                return this.BadRequest("ERROR_INVALID_COMPANY");
            }

            BusinessResult<PagedList<UserUiModel>> fetchedUsers;

            try
            {
                Log.Information(
                    $"--Method:GetUsersAsync -- Message:USERS_FETCH" +
                    $" -- Datetime:{DateTime.Now} -- Just Before : GetUsersAsync");
                fetchedUsers = await this._mediator.Send(new GetUsersByDepartmentQuery(departmentId, parameters));
                Log.Information(
                    $"--Method:GetUsersAsync -- Message:UserS_FETCH" +
                    $" -- Datetime:{DateTime.Now} -- Just After : GetUsersAsync");
            }
            catch (Exception e)
            {
                Log.Error(
                    $"--Method:GetUsersAsync -- Message:UserS_FETCH_ERROR" +
                    $" -- Datetime:{DateTime.Now} -- UserInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                return this.BadRequest("ERROR_FETCH_UserS");
            }

            if (fetchedUsers.IsNull())
            {
                return this.NotFound("USER_NOT_FOUND");
            }

            if (!fetchedUsers.IsSuccess())
            {
                return this.OkOrBadRequest(fetchedUsers.BrokenRules);
            }

            if (fetchedUsers.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(fetchedUsers.BrokenRules);
            }

            var responseWithMetaData = this.CreateOkWithMetaData(fetchedUsers.Model, mediaType, parameters, this._urlHelper, "GetUserByIdAsync", "GetUsersAsync");
            return this.Ok(responseWithMetaData);
        }


        /// <summary>
        /// Get - Fetch one User
        /// </summary>
        /// <param name="id">User Id for fetching</param>
        /// <remarks>Fetch one User </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetUserByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetUserByIdAsync(long id)
        {
            var response = await this._mediator.Send(new GetUserByIdQuery(id));

            if (response == null)
            {
                return this.NotFound();
            }

            if (!response.IsSuccess())
            {
                return this.OkOrBadRequest(response.BrokenRules);
            }

            if (response.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(response.BrokenRules);
            }

            return this.Ok(response);
        }


        /// <summary>
        /// Delete - Delete an existing User - Soft Delete
        /// </summary>
        /// <param name="id">User Id for deletion</param>
        /// <param name="request">DeleteUserResourceParameters for deletion</param>
        /// <remarks>Delete existing User </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftUserAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftUserAsync(long id, [FromBody] DeleteUserResourceParameters request)
        {

            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull())
            {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            var response = await this._mediator.Send(new DeleteSoftUserCommand(id, userAudit.Model.Id, request.DeletedReason));

            if (response == null)
            {
                return this.NotFound();
            }

            if (!response.IsSuccess())
            {
                return this.OkOrBadRequest(response.BrokenRules);
            }

            if (response.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(response.BrokenRules);
            }

            return this.Ok(response);
        }

        /// <summary>
        /// Delete - Delete an existing User - Hard Delete
        /// </summary>
        /// <param name="id">User Id for deletion</param>
        /// <remarks>Delete existing User </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardUserAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardUserAsync(long id)
        {
            var response = await this._mediator.Send(new DeleteHardUserCommand(id));

            if (response == null)
            {
                return this.NotFound();
            }

            if (!response.IsSuccess())
            {
                return this.OkOrBadRequest(response.BrokenRules);
            }

            if (response.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(response.BrokenRules);
            }

            return this.Ok(response);
        }

    }//Class : UsersController

}//Namespace : sw.auth.api.Controllers.API.V1