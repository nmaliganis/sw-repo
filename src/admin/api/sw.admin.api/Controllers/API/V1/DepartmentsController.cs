using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.ResourceParameters.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using sw.admin.api.Validators;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.api.Controllers.API.V1
{
    /// <summary>
    /// Department Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class DepartmentsController : BaseController
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
        public DepartmentsController(
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
        /// Get - Fetch all departments
        /// </summary>
        /// <param name="parameters">Department parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all departments </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetDepartmentsAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetDepartmentsAsync(
            [FromQuery] GetDepartmentsResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<DepartmentUiModel, Department>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<DepartmentUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetDepartmentsQuery(parameters));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetDepartmentByIdAsync", "GetDepartmentsAsync");
            return Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one department
        /// </summary>
        /// <param name="id">Department Id for fetching</param>
        /// <remarks>Fetch one department </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetDepartmentByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetDepartmentByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetDepartmentByIdQuery(id));

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
        /// Post - Create a department
        /// </summary>
        /// <param name="request">CreateDepartmentResourceParameters for creation</param>
        /// <remarks>Create department </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostDepartmentAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostDepartmentAsync([FromBody] CreateDepartmentResourceParameters request)
        {
            var response = await _mediator.Send(new CreateDepartmentCommand(1, request));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return this.CreatedOrNoResult(response);
        }

        /// <summary>
        /// Put - Update a department
        /// </summary>
        /// <param name="id">Department Id for modification</param>
        /// <param name="request">UpdateDepartmentResourceParameters for modification</param>
        /// <remarks>Update department </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateDepartmentAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateDepartmentAsync(long id, [FromBody] UpdateDepartmentResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateDepartmentCommand(id, 2, request)); //(id, modifyedbyID, params)

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
        /// Delete - Delete an existing department - Soft Delete
        /// </summary>
        /// <param name="id">Department Id for deletion</param>
        /// <param name="request">DeleteDepartmentResourceParameters for deletion</param>
        /// <remarks>Delete existing department </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftDepartmentAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftDepartmentAsync(long id, [FromBody] DeleteDepartmentResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftDepartmentCommand(id, deletedBy, request.DeletedReason));

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
        /// Delete - Delete an existing department - Hard Delete
        /// </summary>
        /// <param name="id">Department Id for deletion</param>
        /// <remarks>Delete existing department </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardDepartmentAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardDepartmentAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardDepartmentCommand(id));

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
