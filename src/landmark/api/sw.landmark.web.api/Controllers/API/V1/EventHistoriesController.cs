using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.common.dtos.V1.ResourseParameters.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
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
    /// EventHistory Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class EventHistoriesController : BaseController
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
        public EventHistoriesController(
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
        /// Get - Fetch all event histories
        /// </summary>
        /// <param name="parameters">EventHistory parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all event histories </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetEventHistoriesAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetEventHistoriesAsync(
            [FromQuery] GetEventHistoriesResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetEventHistoriesQuery(parameters));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetEventHistoryByIdAsync", "GetEventHistoriesAsync");
            return Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one event history
        /// </summary>
        /// <param name="id">EventHistory Id for fetching</param>
        /// <remarks>Fetch one event history </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetEventHistoryByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetEventHistoryByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetEventHistoryByIdQuery(id));

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
        /// Post - Create a event history
        /// </summary>
        /// <param name="request">CreateEventHistoryResourceParameters for creation</param>
        /// <remarks>Create event history </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostEventHistoryAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostEventHistoryAsync([FromBody] CreateEventHistoryResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateEventHistoryCommand(1, request) // TODO: Get user id from auth
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
        /// Put - Update a event history
        /// </summary>
        /// <param name="id">EventHistory Id for modification</param>
        /// <param name="request">UpdateEventHistoryResourceParameters for modification</param>
        /// <remarks>Update event history </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateEventHistoryAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateEventHistoryAsync(long id, [FromBody] UpdateEventHistoryResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateEventHistoryCommand(id, 2, request)); // TODO: Get user id from auth

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
        /// Delete - Delete an existing event history - Soft Delete
        /// </summary>
        /// <param name="id">EventHistory Id for deletion</param>
        /// <param name="request">DeleteEventHistoryResourceParameters for deletion</param>
        /// <remarks>Delete existing event history </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftEventHistoryAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftEventHistoryAsync(long id, [FromBody] DeleteEventHistoryResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftEventHistoryCommand(id, deletedBy, request.DeletedReason));

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
        /// Delete - Delete an existing event history - Hard Delete
        /// </summary>
        /// <param name="id">EventHistory Id for deletion</param>
        /// <remarks>Delete existing event history </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardEventHistoryAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardEventHistoryAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardEventHistoryCommand(id));

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
