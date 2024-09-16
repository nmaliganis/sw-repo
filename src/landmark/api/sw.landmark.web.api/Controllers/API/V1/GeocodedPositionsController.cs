using sw.landmark.common.dtos.V1.Cqrs.GeocodedPositions;
using sw.landmark.common.dtos.V1.ResourseParameters.GeocodedPositions;
using sw.landmark.common.dtos.V1.Vms.GeocodedPositions;
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
    /// GeocodedPosition Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class GeocodedPositionsController : BaseController
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
        public GeocodedPositionsController(
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
        /// Get - Fetch all geocoded positions
        /// </summary>
        /// <param name="parameters">GeocodedPosition parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all geocoded positions </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetGeocodedPositionsAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetGeocodedPositionsAsync(
            [FromQuery] GetGeocodedPositionsResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<GeocodedPositionUiModel, GeocodedPosition>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<GeocodedPositionUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetGeocodedPositionsQuery(parameters));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetGeocodedPositionByIdAsync", "GetGeocodedPositionsAsync");
            return Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one geocoded position
        /// </summary>
        /// <param name="id">GeocodedPosition Id for fetching</param>
        /// <remarks>Fetch one geocoded position </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetGeocodedPositionByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetGeocodedPositionByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetGeocodedPositionByIdQuery(id));

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
        /// Post - Create a geocoded position
        /// </summary>
        /// <param name="request">CreateGeocodedPositionResourceParameters for creation</param>
        /// <remarks>Create geocoded position </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostGeocodedPositionAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostGeocodedPositionAsync([FromBody] CreateGeocodedPositionResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateGeocodedPositionCommand(1, request) // TODO: Get user id from auth
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
        /// Put - Update a geocoded position
        /// </summary>
        /// <param name="id">GeocodedPosition Id for modification</param>
        /// <param name="request">UpdateGeocodedPositionResourceParameters for modification</param>
        /// <remarks>Update geocoded position </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateGeocodedPositionAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateGeocodedPositionAsync(long id, [FromBody] UpdateGeocodedPositionResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateGeocodedPositionCommand(id, 2, request)); // TODO: Get user id from auth

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
        /// Delete - Delete an existing geocoded position - Soft Delete
        /// </summary>
        /// <param name="id">GeocodedPosition Id for deletion</param>
        /// <param name="request">DeleteGeocodedPositionResourceParameters for deletion</param>
        /// <remarks>Delete existing geocoded position </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftGeocodedPositionAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftGeocodedPositionAsync(long id, [FromBody] DeleteGeocodedPositionResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftGeocodedPositionCommand(id, deletedBy, request.DeletedReason));

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
        /// Delete - Delete an existing geocoded position - Hard Delete
        /// </summary>
        /// <param name="id">GeocodedPosition Id for deletion</param>
        /// <remarks>Delete existing geocoded position </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardGeocodedPositionAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardGeocodedPositionAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardGeocodedPositionCommand(id));

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
