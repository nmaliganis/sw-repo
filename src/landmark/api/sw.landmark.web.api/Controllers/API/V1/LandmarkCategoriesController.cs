using sw.landmark.common.dtos.V1.Cqrs.LandmarkCategories;
using sw.landmark.common.dtos.V1.ResourseParameters.LandmarkCategories;
using sw.landmark.common.dtos.V1.Vms.LandmarkCategories;
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
    /// LandmarkCategory Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class LandmarkCategoriesController : BaseController
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
        public LandmarkCategoriesController(
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
        /// Get - Fetch all landmark categories
        /// </summary>
        /// <param name="parameters">LandmarkCategory parameters for fetching</param>
        /// <param name="mediaType"></param>
        /// <remarks>Fetch all landmark categories </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetLandmarkCategoriesAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetLandmarkCategoriesAsync(
            [FromQuery] GetLandmarkCategoriesResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<LandmarkCategoryUiModel, LandmarkCategory>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<LandmarkCategoryUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetLandmarkCategoriesQuery(parameters));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetLandmarkCategoryByIdAsync", "GetLandmarkCategoriesAsync");
            return Ok(responseWithMetaData);
        }

        /// <summary>
        /// Get - Fetch one landmark category
        /// </summary>
        /// <param name="id">LandmarkCategory Id for fetching</param>
        /// <remarks>Fetch one landmark category </remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetLandmarkCategoryByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetLandmarkCategoryByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetLandmarkCategoryByIdQuery(id));

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
        /// Post - Create a landmark category
        /// </summary>
        /// <param name="request">CreateLandmarkCategoryResourceParameters for creation</param>
        /// <remarks>Create landmark category </remarks>
        /// <response code="200">Resource Creation Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost(Name = "PostLandmarkCategoryAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostLandmarkCategoryAsync([FromBody] CreateLandmarkCategoryResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateLandmarkCategoryCommand(1, request) // TODO: Get user id from auth
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
        /// Put - Update a landmark category
        /// </summary>
        /// <param name="id">LandmarkCategory Id for modification</param>
        /// <param name="request">UpdateLandmarkCategoryResourceParameters for modification</param>
        /// <remarks>Update landmark category </remarks>
        /// <response code="200">Resource Modification Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateLandmarkCategoryAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateLandmarkCategoryAsync(long id, [FromBody] UpdateLandmarkCategoryResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateLandmarkCategoryCommand(id, 2, request)); // TODO: Get user id from auth

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
        /// Delete - Delete an existing landmark category - Soft Delete
        /// </summary>
        /// <param name="id">LandmarkCategory Id for deletion</param>
        /// <param name="request">DeleteLandmarkCategoryResourceParameters for deletion</param>
        /// <remarks>Delete existing landmark category </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("soft/{id}", Name = "DeleteSoftLandmarkCategoryAsync")]
        [ValidateModel]
        public async Task<IActionResult> DeleteSoftLandmarkCategoryAsync(long id, [FromBody] DeleteLandmarkCategoryResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftLandmarkCategoryCommand(id, deletedBy, request.DeletedReason));

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
        /// Delete - Delete an existing landmark category - Hard Delete
        /// </summary>
        /// <param name="id">LandmarkCategory Id for deletion</param>
        /// <remarks>Delete existing landmark category </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "DeleteHardLandmarkCategoryAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardLandmarkCategoryAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardLandmarkCategoryCommand(id));

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
