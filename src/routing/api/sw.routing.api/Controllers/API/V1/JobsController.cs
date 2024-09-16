using sw.routing.api.Validators;
using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.Vms.Jobs;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using sw.routing.model.Jobs;
using sw.infrastructure.Extensions;

namespace sw.routing.api.Controllers.API.V1;

/// <summary>
/// Job Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class JobsController : BaseController
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
    public JobsController(
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
    /// Get - Fetch all Jobs for Itinerary
    /// </summary>
    /// <param name="parameters">Job parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all Jobs from Itinerary </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("itineraries/{itineraryId}", Name = "FetchJobsFromItineraryRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(BusinessResult<PagedList<JobUiModel>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FetchJobsFromItineraryAsync(long itineraryId, [FromQuery] GetItinerariesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        return Ok();
    }
}//Class : JobsController