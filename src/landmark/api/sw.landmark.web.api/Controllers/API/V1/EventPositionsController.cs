using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.common.dtos.V1.ResourseParameters.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using sw.landmark.api.Validators;
using sw.infrastructure.Controlles.Base;

namespace sw.landmark.web.api.Controllers.API.V1
{
    /// <summary>
    /// EventPosition Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class EventPositionsController : BaseController
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
        public EventPositionsController(
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
        /// Get - Fetch all event positions
        /// </summary>
        /// <param name="parameters">EventPosition parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all event positions </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetEventPositionsAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetEventPositionsAsync(
            [FromQuery] GetEventPositionsResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventPositionUiModel, EventPosition>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<EventPositionUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetEventPositionsQuery(parameters));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetEventPositionByIdAsync", "GetEventPositionsAsync");
            return Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one event position
        /// </summary>
        /// <param name="id">EventPosition Id for fetching</param>
        /// <remarks>Fetch one event position </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetEventPositionByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetEventPositionByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetEventPositionByIdQuery(id));

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
        /// Post - Create a event position
        /// </summary>
        /// <param name="request">CreateEventPositionResourceParameters for creation</param>
        /// <remarks>Create event position </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostEventPositionAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostEventPositionAsync([FromBody] CreateEventPositionResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateEventPositionCommand(1, request) // TODO: Get user id from auth
            );

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
        /// Put - Update a event position
        /// </summary>
        /// <param name="id">EventPosition Id for modification</param>
        /// <param name="request">UpdateEventPositionResourceParameters for modification</param>
        /// <remarks>Update event position </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateEventPositionAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateEventPositionAsync(long id, [FromBody] UpdateEventPositionResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateEventPositionCommand(id, 2, request)); // TODO: Get user id from auth

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
        /// Delete - Delete an existing event position - Soft Delete
        /// </summary>
        /// <param name="id">EventPosition Id for deletion</param>
        /// <param name="request">DeleteEventPositionResourceParameters for deletion</param>
        /// <remarks>Delete existing event position </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftEventPositionAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftEventPositionAsync(long id, [FromBody] DeleteEventPositionResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftEventPositionCommand(id, deletedBy, request.DeletedReason));

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
        /// Delete - Delete an existing event position - Hard Delete
        /// </summary>
        /// <param name="id">EventPosition Id for deletion</param>
        /// <remarks>Delete existing event position </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardEventPositionAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardEventPositionAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardEventPositionCommand(id));

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
