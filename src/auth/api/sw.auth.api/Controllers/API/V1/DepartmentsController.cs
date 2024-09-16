using sw.auth.api.Validators;
using sw.auth.model.Departments;
using sw.common.dtos.ResourceParameters.Departments;
using sw.common.dtos.Vms.Departments;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Departments;
using sw.auth.common.dtos.Vms.Users;

namespace sw.auth.api.Controllers.API.V1 {
    /// <summary>
    /// Class : Department Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Roles = "SU, ADMIN")]
    public class DepartmentsController : BaseController {
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
        public DepartmentsController(
            IMediator mediator,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingService propertyMappingService) {
            this._mediator = mediator;
            this._urlHelper = urlHelper;
            this._typeHelperService = typeHelperService;
            this._propertyMappingService = propertyMappingService;
        }

        /// <summary>
        /// Get - Fetch all Departments
        /// </summary>
        /// <param name="parameters">Department parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all Departments </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetDepartmentsAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetDepartmentsAsync(
            [FromQuery] GetDepartmentsResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType) {

            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull()) {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<DepartmentUiModel, Department>(parameters.OrderBy)) {
                return this.BadRequest("DEPARTMENTS_MODEL_ERROR");
            }

            if (!this._typeHelperService.TypeHasProperties<DepartmentUiModel>
                (parameters.Fields)) {
                return this.BadRequest("DEPARTMENTS_FIELDS_ERROR");
            }

            var companyIdFromClaims = this.GetCompanyFromClaims();

            if (companyIdFromClaims <= 0)
            {
                return this.BadRequest("ERROR_INVALID_COMPANY");
            }

            BusinessResult<PagedList<DepartmentUiModel>> fetchedDepartments;
            if (this.IsSuFromClaims()) {
                try {
                    Log.Information(
                        $"--Method:GetDepartmentsAsync -- Message:DEPARTMENTS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just Before : GetDepartmentsAsync");
                    fetchedDepartments = await this._mediator.Send(new GetDepartmentsQuery(parameters));
                    Log.Information(
                        $"--Method:GetDepartmentsAsync -- Message:DEPARTMENTS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just After : GetDepartmentsAsync");
                } catch (Exception e) {
                    Log.Error(
                        $"--Method:GetDepartmentsAsync -- Message:DEPARTMENTS_FETCH_ERROR" +
                        $" -- Datetime:{DateTime.Now} -- DepartmentInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                    return this.BadRequest("ERROR_FETCH_DEPARTMENTS");
                }
            } else {
                try {
                    Log.Information(
                        $"--Method:GetDepartmentsAsync -- Message:DEPARTMENTS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just Before : GetDepartmentsAsync");
                    fetchedDepartments = await this._mediator.Send(new GetDepartmentsByCompanyQuery(companyIdFromClaims, parameters));
                    Log.Information(
                        $"--Method:GetDepartmentsAsync -- Message:DEPARTMENTS_FETCH" +
                        $" -- Datetime:{DateTime.Now} -- Just After : GetDepartmentsAsync");
                } catch (Exception e) {
                    Log.Error(
                        $"--Method:GetDepartmentsAsync -- Message:DEPARTMENTS_FETCH_ERROR" +
                        $" -- Datetime:{DateTime.Now} -- DepartmentInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                    return this.BadRequest("ERROR_FETCH_DEPARTMENTS");
                }
            }

            if (fetchedDepartments.IsNull()) {
                return this.NotFound("DEPARTMENTS_NOT_FOUND");
            }

            if (!fetchedDepartments.IsSuccess()) {
                return this.OkOrBadRequest(fetchedDepartments.BrokenRules);
            }

            if (fetchedDepartments.Status == BusinessResultStatus.Fail) {
                return this.OkOrNoResult(fetchedDepartments.BrokenRules);
            }

            var responseWithMetaData = this.CreateOkWithMetaData(fetchedDepartments.Model, mediaType,
                parameters, this._urlHelper, "GetDepartmentByIdAsync", "GetDepartmentsAsync");
            return this.Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one Department
        /// </summary>
        /// <param name="id">Department Id for fetching</param>
        /// <remarks>Fetch one Department </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetDepartmentByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetDepartmentByIdAsync(long id) {

            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull()) {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            var fetchedDepartment = await this._mediator.Send(new GetDepartmentByIdQuery(id));

            if (fetchedDepartment.IsNull()) {
                return this.NotFound("DEPARTMENT_NOT_FOUND");
            }

            if (!fetchedDepartment.IsSuccess()) {
                return this.OkOrBadRequest(fetchedDepartment.BrokenRules);
            }

            if (fetchedDepartment.Status == BusinessResultStatus.Fail) {
                return this.OkOrNoResult(fetchedDepartment.BrokenRules);
            }

            return this.Ok(fetchedDepartment);
        }

        /// <summary>
        /// Post - Create a Department
        /// </summary>
        /// <param name="request">CreateDepartmentResourceParameters for creation</param>
        /// <remarks>Create Department </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostDepartmentAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostDepartmentAsync([FromBody] CreateDepartmentResourceParameters request) {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull()) {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            var createdDepartment = await this._mediator.Send(
                new CreateDepartmentCommand(userAudit.Model.Id, request)
            );

            if (createdDepartment.IsNull()) {
                return this.NotFound("ERROR_DEPARTMENT_CREATED_NOT_FOUND");
            }

            if (!createdDepartment.IsSuccess()) {
                return this.OkOrBadRequest(createdDepartment.BrokenRules);
            }

            if (createdDepartment.Status == BusinessResultStatus.Fail) {
                return this.OkOrNoResult(createdDepartment.BrokenRules);
            }

            return this.CreatedOrNoResult(createdDepartment);
        }

        /// <summary>
        /// Put - Update a Department
        /// </summary>
        /// <param name="id">Department Id for modification</param>
        /// <param name="request">UpdateDepartmentResourceParameters for modification</param>
        /// <remarks>Update Department </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateDepartmentAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateDepartmentAsync(long id, [FromBody] UpdateDepartmentResourceParameters request) {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.IsNull()) {
                return this.NotFound("USER_AUDIT_NOT_FOUND");
            }

            var response = await this._mediator.Send(new UpdateDepartmentCommand(id, userAudit.Model.Id, request));

            if (response == null) {
                return this.NotFound("DEPARTMENT_NOT_FOUNT");
            }

            if (!response.IsSuccess()) {
                return this.OkOrBadRequest(response.BrokenRules);
            }

            if (response.Status == BusinessResultStatus.Fail) {
                return this.OkOrNoResult(response.BrokenRules);
            }

            return this.Ok(response);
        }

        /// <summary>
        /// Delete - Delete an existing Department - Soft Delete
        /// </summary>
        /// <param name="id">Department Id for deletion</param>
        /// <param name="request">DeleteDepartmentResourceParameters for deletion</param>
        /// <remarks>Delete existing Department </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftDepartmentAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftDepartmentAsync(long id, [FromBody] DeleteDepartmentResourceParameters request) {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.Model == null)
            {
                return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
            }

            var response = await this._mediator.Send(new DeleteSoftDepartmentCommand(id, userAudit.Model.Id, request.DeletedReason));

            if (response.IsNull()) {
                return this.NotFound("DEPARTMENT_NOT_FOUNT");
            }

            if (!response.IsSuccess()) {
                return this.OkOrBadRequest(response.BrokenRules);
            }

            if (response.Status == BusinessResultStatus.Fail) {
                return this.OkOrNoResult(response.BrokenRules);
            }

            return this.Ok(response);
        }

        /// <summary>
        /// Delete - Delete an existing Department - Hard Delete
        /// </summary>
        /// <param name="id">Department Id for deletion</param>
        /// <remarks>Delete existing Department </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardDepartmentAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardDepartmentAsync(long id) {
            var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

            if (userAudit.Model == null)
            {
                return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
            }

            if (!this.IsSuFromClaims())
            {
                Log.Error(
                    $"--Method:GetDepartmentsRoot -- Message:DEPARTMENT_HARD_DELETION" +
                    $" -- Datetime:{DateTime.Now} -- Action: GetDepartmentsAsync");
                return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
            }

            var response = await this._mediator.Send(new DeleteHardDepartmentCommand(id));

            if (response.IsNull()) {
                return this.NotFound();
            }

            if (!response.IsSuccess()) {
                return this.OkOrBadRequest(response.BrokenRules);
            }

            if (response.Status == BusinessResultStatus.Fail) {
                return this.OkOrNoResult(response.BrokenRules);
            }

            return this.Ok(response);
        }
    }
}
