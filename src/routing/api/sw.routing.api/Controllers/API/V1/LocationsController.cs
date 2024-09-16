using sw.routing.api.Validators;
using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Locations;
using sw.routing.common.dtos.ResourceParameters.Locations;
using sw.routing.common.dtos.Vms.Locations;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.Extensions;

namespace sw.routing.api.Controllers.API.V1;

/// <summary>
/// Location Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(Roles = "SU, ADMIN")]
public class LocationsController : BaseController
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
    public LocationsController(
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
    /// Get - Fetch all Locations
    /// </summary>
    /// <param name="parameters">Location parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all Locations </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "FetchLocationsRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(BusinessResult<PagedList<LocationUiModel>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FetchLocationsAsync(
        [FromQuery] GetItinerariesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<LocationUiModel, LocationPoint>(parameters.OrderBy))
        {
            return this.BadRequest("ERROR_RESOURCE_PARAMETER");
        }

        if (!this._typeHelperService.TypeHasProperties<LocationUiModel>
                (parameters.Fields))
        {
            return this.BadRequest("ERROR_FIELDS_PARAMETER");
        }

        var fetchedLocations = await this._mediator.Send(new GetLocationsQuery(parameters));

        if (fetchedLocations.IsNull())
        {
            return this.NotFound("ERROR_FETCH_SPECIAL_DAYS");
        }

        if (!fetchedLocations.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedLocations.BrokenRules);
        }

        if (fetchedLocations.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedLocations.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedLocations.Model, mediaType,
            parameters, this._urlHelper, "GetLocationByIdAsync", "FetchLocationsAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one Location
    /// </summary>
    /// <param name="id">Location Id for fetching</param>
    /// <remarks>Fetch one Location </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetLocationByIdAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(LocationUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetLocationByIdAsync(long id)
    {
        var fetchedLocation = await this._mediator.Send(new GetLocationByIdQuery(id));

        if (fetchedLocation.IsNull())
        {
            return this.NotFound("ERROR_FETCH_SPECIAL_DAY");
        }

        if (!fetchedLocation.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedLocation.BrokenRules);
        }

        if (fetchedLocation.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedLocation.BrokenRules);
        }

        return this.Ok(fetchedLocation);
    }

    /// <summary>
    /// Post - Create a Location
    /// </summary>
    /// <param name="LocationForCreation">CreateLocationResourceParameters for creation</param>
    /// <remarks>Create Location </remarks>
    /// <response code="201">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostLocationAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(LocationUiModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PostLocationAsync([FromBody] CreateLocationResourceParameters LocationForCreation)
    {
        if (!this.IsSuFromClaims())
        {
            return this.BadRequest("ERROR_UNAUTHORIZED_FOR_CREATION_ACTION");
        }

        //var createdLocation = await this._mediator.Send(new CreateLocationCommand(userAudit.Model.Id, LocationForCreation.Name));
        var createdLocation = await this._mediator.Send(new CreateLocationCommand(0, LocationForCreation.Name));

        if (createdLocation.IsNull())
        {
            return this.NotFound("ERROR_SPECIAL_DAY_CREATED_NOT_FOUND");
        }

        if (!createdLocation.IsSuccess())
        {
            return this.OkOrBadRequest(createdLocation.BrokenRules);
        }

        if (createdLocation.Status == BusinessResultStatus.Fail)
        {
            Log.Information(
                $"--Method:PostLocationRouteAsync -- Message:SPECIAL_DAY_CREATION_SUCCESSFULLY" +
                $" -- Datetime:{DateTime.Now} -- LocationInfo:{createdLocation.Model.Name}");
            return this.OkOrNoResult(createdLocation.BrokenRules);
        }
        return this.CreatedOrNoResult(createdLocation);
    }

    /// <summary>
    /// Put - Update a Location
    /// </summary>
    /// <param name="id">Location Id for modification</param>
    /// <param name="request">UpdateLocationResourceParameters for modification</param>
    /// <remarks>Update Location </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateLocationAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(LocationUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateLocationAsync(long id, [FromBody] UpdateLocationResourceParameters request)
    {
        //var response = await this._mediator.Send(new UpdateLocationCommand(userAudit.Model.Id, id, request.Name));
        var response = await this._mediator.Send(new UpdateLocationCommand(0, id, request.Name));

        if (response == null)
        {
            return this.NotFound("SPECIAL_DAY_NOT_FOUNT");
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
    /// Delete - Delete an existing Location - Soft Delete
    /// </summary>
    /// <param name="id">Location Id for deletion</param>
    /// <param name="request">DeleteLocationResourceParameters for deletion</param>
    /// <remarks>Delete existing Location </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftLocationAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(LocationUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteSoftLocationAsync(long id, [FromBody] DeleteLocationResourceParameters request)
    {
        //var deletedLocation = await this._mediator.Send(new DeleteSoftLocationCommand(id, userAudit.Model.Id, request.DeletedReason));
        var deletedLocation = await this._mediator.Send(new DeleteSoftLocationCommand(id, 0, request.DeletedReason));

        if (deletedLocation.IsNull())
        {
            return this.NotFound("ERROR_DELETE_SPECIAL_DAY");
        }

        if (!deletedLocation.IsSuccess())
        {
            return this.OkOrBadRequest(deletedLocation.BrokenRules);
        }

        if (deletedLocation.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(deletedLocation.BrokenRules);
        }

        return this.Ok(deletedLocation);
    }

    /// <summary>
    /// Delete - Delete an existing Location - Hard Delete
    /// </summary>
    /// <param name="id">Location Id for deletion</param>
    /// <remarks>Delete existing Location </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardLocationAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(LocationUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteHardLocationAsync(long id)
    {
        //var response = await this._mediator.Send(new DeleteHardLocationCommand(id));
        var response = await this._mediator.Send(new DeleteHardLocationCommand(id));

        if (response.IsNull())
        {
            return this.NotFound("ERROR_DELETE_SPECIAL_DAY");
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
}//Class : LocationsController