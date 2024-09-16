using sw.landmark.common.dtos.V1.Cqrs.Landmarks;
using sw.landmark.common.dtos.V1.ResourseParameters.Landmarks;
using sw.landmark.common.dtos.V1.Vms.Landmarks;
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
    /// Landmark Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class LandmarksController : BaseController
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
        public LandmarksController(
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
        /// Get - Fetch all landmarks
        /// </summary>
        /// <param name="parameters">Landmark parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all landmarks </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetLandmarksAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetLandmarksAsync(
            [FromQuery] GetLandmarksResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<LandmarkUiModel, Landmark>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<LandmarkUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetLandmarksQuery(parameters));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetLandmarkByIdAsync", "GetLandmarksAsync");
            return Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one landmark
        /// </summary>
        /// <param name="id">Landmark Id for fetching</param>
        /// <remarks>Fetch one landmark </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetLandmarkByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetLandmarkByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetLandmarkByIdQuery(id));

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
        /// Post - Create a landmark
        /// </summary>
        /// <param name="request">CreateLandmarkResourceParameters for creation</param>
        /// <remarks>Create landmark </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostLandmarkAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostLandmarkAsync([FromBody] CreateLandmarkResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateLandmarkCommand(1, request) // TODO: Get user id from auth
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
        /// Put - Update a landmark
        /// </summary>
        /// <param name="id">Landmark Id for modification</param>
        /// <param name="request">UpdateLandmarkResourceParameters for modification</param>
        /// <remarks>Update landmark </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateLandmarkAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateLandmarkAsync(long id, [FromBody] UpdateLandmarkResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateLandmarkCommand(id, 2, request)); // TODO: Get user id from auth

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
        /// Delete - Delete an existing landmark - Soft Delete
        /// </summary>
        /// <param name="id">Landmark Id for deletion</param>
        /// <param name="request">DeleteLandmarkResourceParameters for deletion</param>
        /// <remarks>Delete existing landmark </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftLandmarkAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftLandmarkAsync(long id, [FromBody] DeleteLandmarkResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftLandmarkCommand(id, deletedBy, request.DeletedReason));

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
        /// Delete - Delete an existing landmark - Hard Delete
        /// </summary>
        /// <param name="id">Landmark Id for deletion</param>
        /// <remarks>Delete existing landmark </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardLandmarkAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardLandmarkAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardLandmarkCommand(id));

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
