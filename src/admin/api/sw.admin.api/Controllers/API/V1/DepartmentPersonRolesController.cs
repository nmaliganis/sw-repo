using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.ResourceParameters.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using sw.admin.api.Validators;

namespace sw.admin.api.Controllers.API.V1
{
    /// <summary>
    /// DepartmentPersonRole Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class DepartmentPersonRolesController : BaseController
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
        public DepartmentPersonRolesController(
            IMediator mediator,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingService propertyMappingService)
        {
            _mediator = mediator;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;
        }

        /// <summary>
        /// Get - Fetch all department person roles
        /// </summary>
        /// <param name="parameters">DepartmentPersonRole parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all department person roles </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetDepartmentPersonRolesAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetDepartmentPersonRolesAsync(
            [FromQuery] GetDepartmentPersonRolesResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<DepartmentPersonRoleUiModel, DepartmentPersonRole>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<DepartmentPersonRoleUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetDepartmentPersonRolesQuery(parameters));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetDepartmentPersonRoleByIdAsync", "GetDepartmentPersonRolesAsync");
            return Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one department person role
        /// </summary>
        /// <param name="id">DepartmentPersonRole Id for fetching</param>
        /// <remarks>Fetch one department person role </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetDepartmentPersonRoleByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetDepartmentPersonRoleByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetDepartmentPersonRoleByIdQuery(id));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return Ok(response);
        }

        /// <summary>
        /// Post - Create a department person role
        /// </summary>
        /// <param name="request">CreateDepartmentPersonRoleResourceParameters for creation</param>
        /// <remarks>Create department person role </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostDepartmentPersonRoleAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostDepartmentPersonRoleAsync([FromBody] CreateDepartmentPersonRoleResourceParameters request)
        {
            var response = await _mediator.Send(new CreateDepartmentPersonRoleCommand(1, request));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return Ok(response);
        }

        /// <summary>
        /// Put - Update a department person role
        /// </summary>
        /// <param name="id">DepartmentPersonRole Id for modification</param>
        /// <param name="request">UpdateDepartmentPersonRoleResourceParameters for modification</param>
        /// <remarks>Update department person role </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateDepartmentPersonRoleAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateDepartmentPersonRoleAsync(long id, [FromBody] UpdateDepartmentPersonRoleResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateDepartmentPersonRoleCommand(2, id, request));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return Ok(response);
        }

        /// <summary>
        /// Delete - Delete an existing department person role - Soft Delete
        /// </summary>
        /// <param name="id">DepartmentPersonRole Id for deletion</param>
        /// <param name="request">DeleteDepartmentPersonRoleResourceParameters for deletion</param>
        /// <remarks>Delete existing department person role </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftDepartmentPersonRoleAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftDepartmentPersonRoleAsync(long id, [FromBody] DeleteDepartmentPersonRoleResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftDepartmentPersonRoleCommand(id, deletedBy, request.DeletedReason));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return Ok(response);
        }

        /// <summary>
        /// Delete - Delete an existing department person role - Hard Delete
        /// </summary>
        /// <param name="id">DepartmentPersonRole Id for deletion</param>
        /// <remarks>Delete existing department person role </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardDepartmentPersonRoleAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardDepartmentPersonRoleAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardDepartmentPersonRoleCommand(id));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return Ok(response);
        }
    }
}
