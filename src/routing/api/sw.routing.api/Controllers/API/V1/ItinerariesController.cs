using System;
using System.Net;
using System.Threading.Tasks;
using sw.routing.api.Validators;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.model.Itineraries;
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

namespace sw.routing.api.Controllers.API.V1;

/// <summary>
/// Itinerary Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class ItinerariesController : BaseController
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
    public ItinerariesController(
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
    /// Get - Fetch all Itineraries
    /// </summary>
    /// <param name="parameters">Itinerary parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all Itineraries </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "FetchItinerariesRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(BusinessResult<PagedList<ItineraryUiModel>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FetchItinerariesAsync(
        [FromQuery] GetItinerariesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<ItineraryUiModel, Itinerary>(parameters.OrderBy))
        {
            return this.BadRequest("ERROR_RESOURCE_PARAMETER");
        }

        if (!this._typeHelperService.TypeHasProperties<ItineraryUiModel>
                (parameters.Fields))
        {
            return this.BadRequest("ERROR_FIELDS_PARAMETER");
        }

        var fetchedItineraries = await this._mediator.Send(new GetItinerariesQuery(parameters));

        if (fetchedItineraries.IsNull())
        {
            return this.NotFound("ERROR_FETCH_ITINERARIES");
        }

        if (!fetchedItineraries.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedItineraries.BrokenRules);
        }

        if (fetchedItineraries.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedItineraries.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedItineraries.Model, mediaType,
            parameters, this._urlHelper, "GetItineraryByIdAsync", "FetchItinerariesAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one Itinerary
    /// </summary>
    /// <param name="id">Itinerary Id for fetching</param>
    /// <remarks>Fetch one Itinerary </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetItineraryByIdAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetItineraryByIdAsync(long id)
    {
        var fetchedItinerary = await this._mediator.Send(new GetItineraryByIdQuery(id));

        if (fetchedItinerary.IsNull())
        {
            return this.NotFound("ERROR_FETCH_ITINERARY");
        }

        if (!fetchedItinerary.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedItinerary.BrokenRules);
        }

        if (fetchedItinerary.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedItinerary.BrokenRules);
        }

        return this.Ok(fetchedItinerary);
    }

    /// <summary>
    /// Post - Create a Itinerary
    /// </summary>
    /// <param name="itineraryForCreation">CreateItineraryResourceParameters for creation</param>
    /// <remarks>Create Itinerary </remarks>
    /// <response code="201">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "CreateItineraryRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryUiModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateItineraryAsync([FromBody] CreateItineraryResourceParameters itineraryForCreation)
    {
        var userAudit = this.GetMemberFromClaims();

        if (userAudit <= 0)
        {
            Log.Error(
                $"--Method:PostItineraryRoot -- Message:PostItineraryAsync" +
                $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var createdItinerary = await this._mediator.Send(new CreateItineraryCommand(userAudit, itineraryForCreation));

        if (createdItinerary.IsNull())
        {
            return this.NotFound("ERROR_ITINERARY_CREATED_NOT_FOUND");
        }

        if (!createdItinerary.IsSuccess())
        {
            return this.OkOrBadRequest(createdItinerary.BrokenRules);
        }

        if (createdItinerary.Status == BusinessResultStatus.Fail)
        {
            Log.Information(
                $"--Method:PostItineraryRouteAsync -- Message:ITINERARY_CREATION_SUCCESSFULLY" +
                $" -- Datetime:{DateTime.Now} -- ItineraryInfo:{createdItinerary.Model.Name}");
            return this.OkOrNoResult(createdItinerary.BrokenRules);
        }
        return this.CreatedOrNoResult(createdItinerary);
    }

    /// <summary>
    /// Put - Update a Itinerary
    /// </summary>
    /// <param name="id">Itinerary Id for modification</param>
    /// <param name="request">UpdateItineraryResourceParameters for modification</param>
    /// <remarks>Update Itinerary </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateItineraryAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateItineraryAsync(long id, [FromBody] UpdateItineraryResourceParameters request)
    {
        var response = await this._mediator.Send(new UpdateItineraryCommand(0, id, request.Name));

        if (response.IsNull())
        {
            return this.NotFound("ITINERARY_NOT_FOUND");
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
    /// Delete - Delete an existing Itinerary - Soft Delete
    /// </summary>
    /// <param name="id">Itinerary Id for deletion</param>
    /// <param name="request">DeleteItineraryResourceParameters for deletion</param>
    /// <remarks>Delete existing Itinerary </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftItineraryAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteSoftItineraryAsync(long id, [FromBody] DeleteItineraryResourceParameters request)
    {
        var deletedItinerary = await this._mediator.Send(new DeleteSoftItineraryCommand(id, 0, request.DeletedReason));

        if (deletedItinerary.IsNull())
        {
            return this.NotFound("ERROR_DELETE_ITINERARY");
        }

        if (!deletedItinerary.IsSuccess())
        {
            return this.OkOrBadRequest(deletedItinerary.BrokenRules);
        }

        if (deletedItinerary.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(deletedItinerary.BrokenRules);
        }

        return this.Ok(deletedItinerary);
    }

    /// <summary>
    /// Delete - Delete an existing Itinerary - Hard Delete
    /// </summary>
    /// <param name="id">Itinerary Id for deletion</param>
    /// <remarks>Delete existing Itinerary </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardItineraryAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteHardItineraryAsync(long id)
    {
        var response = await this._mediator.Send(new DeleteHardItineraryCommand(id));

        if (response.IsNull())
        {
            return this.NotFound("ERROR_DELETE_ITINERARY");
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

}//Class : ItinerarysController