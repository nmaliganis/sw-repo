using sw.auth.api.Validators;
using sw.auth.common.dtos.Cqrs.Roles;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.model.Roles;
using sw.common.dtos.ResourceParameters.Roles;
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
using System.Net;
using System.Threading.Tasks;

namespace sw.auth.api.Controllers.API.V1
{
    /// <summary>
    /// Role Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RolesController : BaseController
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
        public RolesController(
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
        /// Get - Fetch all Roles
        /// </summary>
        /// <param name="parameters">Role parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all Roles </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "FetchRolesRoot")]
        [ValidateModel]
        [Authorize(Roles = "SU, ADMIN")]
        [ProducesResponseType(typeof(BusinessResult<PagedList<RoleUiModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FetchRolesAsync(
            [FromQuery] GetRolesResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {

            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.Model == null)
            {
                return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
            }

            if (!string.IsNullOrEmpty(parameters.OrderBy)
                && !this._propertyMappingService.ValidMappingExistsFor<RoleUiModel, Role>(parameters.OrderBy))
            {
                return this.BadRequest("ERROR_RESOURCE_PARAMETER");
            }

            if (!this._typeHelperService.TypeHasProperties<RoleUiModel>
                    (parameters.Fields))
            {
                return this.BadRequest("ERROR_FIELDS_PARAMETER");
            }

            var fetchedRoles = await this._mediator.Send(new GetRolesQuery(parameters));

            if (fetchedRoles.IsNull())
            {
                return this.NotFound("ERROR_FETCH_ROLES");
            }

            if (!fetchedRoles.IsSuccess())
            {
                return this.OkOrBadRequest(fetchedRoles.BrokenRules);
            }

            if (fetchedRoles.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(fetchedRoles.BrokenRules);
            }

            var responseWithMetaData = this.CreateOkWithMetaData(fetchedRoles.Model, mediaType,
                parameters, this._urlHelper, "GetRoleByIdAsync", "FetchRolesAsync");
            return this.Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one Role
        /// </summary>
        /// <param name="id">Role Id for fetching</param>
        /// <remarks>Fetch one Role </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetRoleByIdAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU, ADMIN")]
        [ProducesResponseType(typeof(RoleUiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRoleByIdAsync(long id)
        {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.Model == null)
            {
                return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
            }

            var fetchedRole = await this._mediator.Send(new GetRoleByIdQuery(id));

            if (fetchedRole.IsNull())
            {
                return this.NotFound("ERROR_FETCH_ROLE");
            }

            if (!fetchedRole.IsSuccess())
            {
                return this.OkOrBadRequest(fetchedRole.BrokenRules);
            }

            if (fetchedRole.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(fetchedRole.BrokenRules);
            }

            return this.Ok(fetchedRole);
        }

        /// <summary>
        /// Post - Create a Role
        /// </summary>
        /// <param name="roleForCreation">CreateRoleResourceParameters for creation</param>
        /// <remarks>Create Role </remarks>
        /// <response code="201">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostRoleAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU, ADMIN")]
        [ProducesResponseType(typeof(RoleUiModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostRoleAsync([FromBody] CreateRoleResourceParameters roleForCreation)
        {

            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull())
            {
                return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
            }

            if (!this.IsSuFromClaims())
            {
                return this.BadRequest("ERROR_UNAUTHORIZED_FOR_CREATION_ACTION");
            }

            var createdRole =
                await this._mediator.Send(new CreateRoleCommand(userAudit.Model.Id, roleForCreation.Name));

            if (createdRole.IsNull())
            {
                return this.NotFound("ERROR_ROLE_CREATED_NOT_FOUND");
            }

            if (!createdRole.IsSuccess())
            {
                return this.OkOrBadRequest(createdRole.BrokenRules);
            }

            if (createdRole.Status == BusinessResultStatus.Fail)
            {
                Log.Information(
                    $"--Method:PostRoleRouteAsync -- Message:ROLE_CREATION_SUCCESSFULLY" +
                    $" -- Datetime:{DateTime.Now} -- RoleInfo:{createdRole.Model.Name}");
                return this.OkOrNoResult(createdRole.BrokenRules);
            }

            return this.CreatedOrNoResult(createdRole);
        }

        /// <summary>
        /// Put - Update a Role
        /// </summary>
        /// <param name="id">Role Id for modification</param>
        /// <param name="request">UpdateRoleResourceParameters for modification</param>
        /// <remarks>Update Role </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateRoleAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU, ADMIN")]
        [ProducesResponseType(typeof(RoleUiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRoleAsync(long id, [FromBody] UpdateRoleResourceParameters request)
        {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull())
            {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            var response = await this._mediator.Send(new UpdateRoleCommand(userAudit.Model.Id, id, request.Name));

            if (response.IsNull())
            {
                return this.NotFound("ROLE_NOT_FOUNT");
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
        /// Delete - Delete an existing Role - Soft Delete
        /// </summary>
        /// <param name="id">Role Id for deletion</param>
        /// <param name="request">DeleteRoleResourceParameters for deletion</param>
        /// <remarks>Delete existing Role </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftRoleAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU, ADMIN")]
        [ProducesResponseType(typeof(RoleUiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteSoftRoleAsync(long id, [FromBody] DeleteRoleResourceParameters request)
        {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.Model == null)
            {
                return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
            }

            var deletedRole =
                await this._mediator.Send(new DeleteSoftRoleCommand(id, userAudit.Model.Id, request.DeletedReason));

            if (deletedRole.IsNull())
            {
                return this.NotFound("ERROR_DELETE_ROLE");
            }

            if (!deletedRole.IsSuccess())
            {
                return this.OkOrBadRequest(deletedRole.BrokenRules);
            }

            if (deletedRole.Status == BusinessResultStatus.Fail)
            {
                return this.OkOrNoResult(deletedRole.BrokenRules);
            }

            return this.Ok(deletedRole);
        }

        /// <summary>
        /// Delete - Delete an existing Role - Hard Delete
        /// </summary>
        /// <param name="id">Role Id for deletion</param>
        /// <remarks>Delete existing Role </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardRoleAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU")]
        [ProducesResponseType(typeof(RoleUiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteHardRoleAsync(long id)
        {

            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.Model.IsNull())
            {
                return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
            }

            var response = await this._mediator.Send(new DeleteHardRoleCommand(id));

            if (response.IsNull())
            {
                return this.NotFound("ERROR_DELETE_ROLE");
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
    } //Class : RolesController
}