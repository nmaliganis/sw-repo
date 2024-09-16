using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.model.Assets.Containers;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace sw.asset.api.Controllers.API.V1;

/// <summary>
/// Container Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class ContainersController : BaseController
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
    public ContainersController(
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
    /// Get - Fetch all containers
    /// </summary>
    /// <param name="parameters">Container parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all containers</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetContainersRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetContainersAsync(
        [FromQuery] GetContainersResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetContainersRoot -- Message:GetEventHistoryAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy)
        && !_propertyMappingService.ValidMappingExistsFor<ContainerUiModel, Container>(parameters.OrderBy))
        {
            return BadRequest("CONTAINER_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<ContainerUiModel>
            (parameters.Fields))
        {
            return BadRequest("CONTAINER_FIELDS_ERROR");
        }

        BusinessResult<PagedList<ContainerUiModel>> response;
        try
        {
            Log.Information(
                $"--Method:GetContainersAsync -- Message:CONTAINERS_FETCH" +
                $" -- Datetime:{DateTime.Now} -- Just Before : GetContainersQuery");
            response =
                await _mediator.Send(new GetContainersQuery(parameters));
            Log.Information(
                $"--Method:GetContainersAsync -- Message:CONTAINERS_FETCH" +
                $" -- Datetime:{DateTime.Now} -- Just After : GetContainersQuery");
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:GetContainersAsync -- Message:CONTAINERS_FETCH_ERROR" +
                $" -- Datetime:{DateTime.Now} -- Container Info:{e.Message} - Details :  {e.InnerException?.Message}");
            return this.BadRequest("ERROR_FETCH_CONTAINERS");
        }


        if (response.IsNull())
        {
            return NotFound("CONTAINERS_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetContainerByIdAsync", "GetContainersAsync");
        return Ok(responseWithMetaData);
    }


    /// <summary>
    /// Get - Fetch all containers by Zone
    /// </summary>
    /// <param name="zoneId"></param>
    /// <param name="parameters">Container parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all containers</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("zones/{zoneId}", Name = "GetContainersByZoneRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetContainersByZoneAsync(long zoneId, [FromQuery] GetContainersResourceParameters parameters, [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetContainersByZoneRoot -- Message:GetContainersByZoneAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy)
        && !_propertyMappingService.ValidMappingExistsFor<ContainerUiModel, Container>(parameters.OrderBy))
        {
            return BadRequest("CONTAINER_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<ContainerUiModel>
            (parameters.Fields))
        {
            return BadRequest("CONTAINER_FIELDS_ERROR");
        }

        BusinessResult<PagedList<ContainerUiModel>> response;
        try
        {
            Log.Information(
                $"--Method:GetContainersByZoneAsync -- Message:CONTAINERS_ZONE_FETCH" +
                $" -- Datetime:{DateTime.Now} -- Just Before : GetContainersByZoneIdQuery");
            response =
                await _mediator.Send(new GetContainersByZoneIdQuery(zoneId, parameters)); Log.Information(
                $"--Method:GetContainersByZoneAsync -- Message:CONTAINERS_ZONE_FETCH" +
                $" -- Datetime:{DateTime.Now} -- Just After : GetContainersByZoneIdQuery");
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:GetContainersByZoneAsync -- Message:CONTAINERS_ZONE_FETCH_ERROR" +
                $" -- Datetime:{DateTime.Now} -- Container Info:{e.Message} - Details :  {e.InnerException?.Message}");
            return this.BadRequest("ERROR_FETCH_CONTAINERS");
        }


        if (response.IsNull())
        {
            return NotFound("CONTAINERS_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetContainersByZoneAsync", "GetContainersByZone");
        return Ok(responseWithMetaData);
    }


    /// <summary>
    /// Get - Fetch all containers by Zones
    /// </summary>
    /// <param name="zones"></param>
    /// <remarks>Fetch all containers</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("zones", Name = "FetchContainersByZonesAsync")]
    [ValidateModel]
    public async Task<IActionResult> FetchContainersByZonesAsync([FromQuery] List<long> zones)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetContainersByZonesRoot -- Message:GetContainersByZonesAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        BusinessResult<List<ContainerUiModel>> response;
        try
        {
            Log.Information(
                $"--Method:FetchContainersByZonesAsync -- Message:CONTAINERS_ZONES_FETCH" +
                $" -- Datetime:{DateTime.Now} -- Just Before : GetContainersByZonesQuery");
            response =
                await _mediator.Send(new GetContainersByZonesQuery(zones));
            Log.Information(
                $"--Method:FetchContainersByZonesAsync -- Message:CONTAINERS_ZONES_FETCH" +
                $" -- Datetime:{DateTime.Now} -- Just After : GetContainersByZonesQuery");
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:FetchContainersByZonesAsync -- Message:CONTAINERS_ZONES_FETCH_ERROR" +
                $" -- Datetime:{DateTime.Now} -- Container Info:{e.Message} - Details :  {e.InnerException?.Message}");
            return this.BadRequest("ERROR_FETCH_CONTAINERS");
        }


        if (response.IsNull())
        {
            return NotFound("CONTAINERS_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = Ok(response.Model);
        return Ok(responseWithMetaData);
    }


    /// <summary>
    /// Get - Retrieve Stored Containers Count Total
    /// </summary>
    /// <remarks>Retrieve Stored Containers Count Totals</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="400">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("count-total", Name = "GetContainersCountTotalsRoot")]
    public async Task<IActionResult> GetContainersCountTotalsAsync([FromQuery] List<long> zones)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetContainersCountTotalsRoot -- Message:GetContainersCountTotalsAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: (userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new GetContainersCountTotalInZonesQuery(zones));
        return Ok(response);
    }

    /// <summary>
    /// Get - Retrieve Stored Containers By Search
    /// </summary>
    /// <remarks>Retrieve Stored Containers By Search</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="400">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("search-by-volume/lower-level/{lowerLevel}/upper-level/{upperLevel}", Name = "GetContainersBySearchVolumeRoot")]
    public async Task<IActionResult> GetContainersBySearchVolumeAsync(int lowerLevel, int upperLevel, [FromQuery] List<long> zones)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
                $"--Method:GetContainersByVolumeRoot -- Message:GetContainersByVolumeAsync" +
                $" -- Datetime:{DateTime.Now} -- Action: (userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new GetContainersByVolumeInZonesQuery(zones, lowerLevel, upperLevel));
        return Ok(response);
    }

    /// <summary>
    /// Get - Retrieve Stored Containers By Search
    /// </summary>
    /// <remarks>Retrieve Stored Containers By Search</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="400">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("search-by-criteria/criteria/{criteria}", Name = "GetContainersBySearchCriteriaRoot")]
    public async Task<IActionResult> GetContainersBySearchCriteriaAsync(string criteria, [FromQuery] List<long> zones)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
                $"--Method:GetContainersBySearchCriteriaRoot -- Message:GetContainersBySearchCriteriaAsync" +
                $" -- Datetime:{DateTime.Now} -- Action: (userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new GetContainersByCriteriaInZonesQuery(zones, criteria));
        return Ok(response);
    }


    /// <summary>
    /// Get - Fetch one container
    /// </summary>
    /// <param name="id">Container Id for fetching</param>
    /// <remarks>Fetch one container </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetContainerByIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetContainerByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetContainerByIdRoot -- Message:GetContainerByIdAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: (userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new GetContainerByIdQuery(id));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:GetContainerByIdRoot -- Message:response.IsNull()" +
              $" -- Datetime:{DateTime.Now} -- Action: GetContainerByIdAsync");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Get - Fetch one container by Device Imei
    /// </summary>
    /// <param name="imei">Container Id for fetching</param>
    /// <remarks>Fetch one container </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("device/{imei}", Name = "GetContainerByImeiRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetContainerByImeiAsync(string imei)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetContainerByImeiRoot -- Message:GetContainerByImeiAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: (userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new GetContainerByImeiQuery(imei));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:GetContainerByImeiRoot -- Message:GetContainerByImeiAsync.IsNull()" +
              $" -- Datetime:{DateTime.Now} -- Action: GetContainerByImeiQuery");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Post - Create a container
    /// </summary>
    /// <param name="request">CreateDeviceResourceParameters for creation</param>
    /// <remarks>Create device </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostContainerRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostContainerAsync([FromBody] CreateContainerResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:PostContainerRoot -- Message:PostContainerAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: PostContainerAsync");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        BusinessResult<ContainerUiModel> createdContainer = await this._mediator.Send(new CreateContainerCommand(userAudit.Model.Id, request));

        if (createdContainer.IsNull())
        {
            Log.Error(
              $"--Method:PostContainerRoot -- Message:CONTAINERS_CREATION" +
              $" -- Datetime:{DateTime.Now} -- Action: PostContainerAsync");
            return this.NotFound("ERROR_CONTAINER_CREATED_NOT_FOUND");
        }

        if (!createdContainer.IsSuccess())
        {
            return this.OkOrBadRequest(createdContainer.BrokenRules);
        }

        if (createdContainer.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(createdContainer.BrokenRules);
        }

        return this.CreatedOrNoResult(createdContainer);
    }


    /// <summary>
    /// Post - Create a container with device IMEI
    /// </summary>
    /// <param name="deviceImei"></param>
    /// <param name="request">CreateContainerResourceParameters for creation</param>
    /// <remarks>Create container </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost("devices/{deviceImei}", Name = "PostContainerWithDeviceImeiRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(BusinessResult<ContainerUiModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PostContainerWithDeviceImeiAsync(string deviceImei, [FromBody] CreateContainerWithDeviceResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        BusinessResult<ContainerUiModel> responseCreation;

        try
        {
            responseCreation = await _mediator.Send(
                new CreateContainerWithDeviceImeiCommand(userAudit.Model.Id, deviceImei, request)
            );
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:PostContainerWithDeviceImeiAsync -- Message:CONTAINER_CREATION_WITH_DEVICE" +
                $" -- Datetime:{DateTime.UtcNow} -- Action: {e.Message}");
            return this.NotFound("CONTAINER_NOT_CREATED");
        }

        if (responseCreation.IsNull())
        {
            Log.Error(
              $"--Method:PostContainerWithDeviceImeiAsync -- Message:CONTAINER_CREATION_WITH_DEVICE" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: response.IsNull()");
            return this.NotFound("CONTAINER_NOT_CREATED");
        }

        if (!responseCreation.IsSuccess())
        {
            Log.Error(
                $"--Method:PostContainerWithDeviceImeiAsync -- Message:CONTAINER_CREATION_WITH_DEVICE" +
                $" -- Datetime:{DateTime.UtcNow} -- Action: {responseCreation.BrokenRules.FirstOrDefault()!.Rule}");
            return this.NotFound("CONTAINER_NOT_CREATED");
        }

        BusinessResult<ContainerModificationUiModel> responseModification;

        try
        {
            responseModification = await _mediator.Send(new UpdateContainerWithLatLonCommand(responseCreation.Model.Id,
                request.Latitude, request.Longitude));
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:PostOnboardingWithDeviceImeiRoot -- Message:CONTAINER_MODIFICATION_WITH_LAT_LON_DEVICE" +
                $" -- Datetime:{DateTime.Now} -- Action: {e.Message}");
            return this.NotFound("CONTAINER_NOT_MODIFIED");
        }


        if (responseModification.IsNull())
        {
            Log.Error(
                $"--Method:PostOnboardingWithDeviceImeiRoot -- Message:CONTAINER_MODIFICATION_WITH_LAT_LON_DEVICE" +
                $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return this.NotFound("CONTAINER_NOT_MODIFIED");
        }

        if (!responseCreation.IsSuccess())
        {
            Log.Error(
                $"--Method:PostContainerWithDeviceImeiAsync -- Message:CONTAINER_CREATION_WITH_DEVICE" +
                $" -- Datetime:{DateTime.Now} -- Action: {responseCreation.BrokenRules.FirstOrDefault()!.Rule}");
            return this.NotFound("CONTAINER_NOT_MODIFIED");
        }

        if (!responseModification.IsSuccess())
            return OkOrBadRequest(responseModification.BrokenRules);

        if (responseModification.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(responseModification.BrokenRules);

        return Ok(responseModification);
    }

    /// <summary>
    /// Put - Update a container - couple it with a device
    /// </summary>
    /// <param name="containerId">Container Id</param>
    /// <param name="deviceId">Device Id</param>
    /// <param name="request">OnboardContainerResourceParameters for creation</param>
    /// <remarks>Create container </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("onboarding/{containerId}/devices/{deviceId}", Name = "PutOnboardingByIdWithDeviceRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(BusinessResult<ContainerModificationUiModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PutOnboardingByIdWithDeviceAsync(long containerId, long deviceId, [FromBody] OnboardingContainerResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(
            new OnboardingContainerWithDeviceCommand(containerId, deviceId, userAudit.Model.Id, request)
        );

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:PutOnboardingByIdWithDeviceRoot -- Message:CONTAINER_UPDATE_WITH_DEVICE" +
              $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUNT");
        }

        if (!response.IsSuccess())
        {
            return OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return OkOrNoResult(response.BrokenRules);
        }

        return Ok(response);
    }

    /// <summary>
    /// Put - Update a container - couple it with a device using its IMEI
    /// </summary>
    /// <param name="containerName">Container Name</param>
    /// <param name="deviceImei">Device Imei</param>
    /// <remarks>Create container </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("onboarding/by-name/{containerName}/devices/by-imei/{deviceImei}", Name = "PutOnboardingByNameWithDeviceImeiRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(BusinessResult<ContainerModificationUiModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PutOnboardingByNameWithDeviceImeiAsync(string containerName, string deviceImei)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(
            new OnboardingContainerWithDeviceByNameCommand(containerName, deviceImei, userAudit.Model.Id)
        );

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:PutOnboardingByNameWithDeviceImeiRoot -- Message:CONTAINER_UPDATE_WITH_DEVICE" +
              $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUNT");
        }

        if (!response.IsSuccess())
        {
            return OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return OkOrNoResult(response.BrokenRules);
        }

        return Ok(response);
    }

    /// <summary>
    /// Put - Update a container
    /// </summary>
    /// <param name="id">Container Id for modification</param>
    /// <param name="request">UpdateContainerResourceParameters for modification</param>
    /// <remarks>Update container </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("by-id/{id}/point", Name = "UpdateContainerWithNewLatLotRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateContainerWithNewLatLotAsync(long id, [FromBody] UpdateContainerWithLatLonResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new UpdateContainerWithLatLonCommand(id,
          request.Lat, request.Lon));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:UpdateContainerWithNewLatLotRoot -- Message:CONTAINER_UPDATE_WITH_DEVICE" +
              $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response.Model);
    }

    /// <summary>
    /// Put - Update a container
    /// </summary>
    /// <param name="deviceImei">Device Imei for modification</param>
    /// <param name="request">UpdateContainerResourceParameters for modification</param>
    /// <remarks>Update container </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("by-device/{deviceImei}/point", Name = "UpdateContainerWithNewLatLotByDeviceImeiRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateContainerWithNewLatLotByDeviceImeiAsync(string deviceImei, [FromBody] UpdateContainerWithLatLonResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new UpdateContainerWithLatLonByDeviceCommand(deviceImei,
            request.Lat, request.Lon));

        if (response.IsNull())
        {
            Log.Error(
                $"--Method:UpdateContainerWithNewLatLotByDeviceImeiRoot -- Message:CONTAINER_UPDATE_WITH_LAT_LON_DEVICE" +
                $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response.Model);
    }


    /// <summary>
    /// Put - Update a container
    /// </summary>
    /// <param name="containerName">Container Name for modification</param>
    /// <param name="request">UpdateContainerResourceParameters for modification</param>
    /// <remarks>Update container </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("by-name/{containerName}/point", Name = "UpdateContainerByNameWithNewLatLotRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateContainerByNameWithNewLatLotAsync(string containerName, [FromBody] UpdateContainerWithLatLonResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new UpdateContainerByNameWithLatLonCommand(containerName,
            request.Lat, request.Lon));

        if (response.IsNull())
        {
            Log.Error(
                $"--Method:UpdateContainerWithNewLatLotRoot -- Message:CONTAINER_UPDATE_WITH_NAME" +
                $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response.Model);
    }


    /// <summary>
    /// Put - Update a container
    /// </summary>
    /// <param name="id">Container Id for modification</param>
    /// <param name="deviceImei">Device Imei</param>
    /// <param name="request">UpdateContainerResourceParameters for modification</param>
    /// <remarks>Update container </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}/devices/{deviceImei}", Name = "UpdateContainerByDeviceImeiRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateContainerByDeviceImeiRootAsync(long id, string deviceImei, [FromBody] UpdateContainerWithMeasurementsResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new UpdateContainerMeasurementsCommand("Ultrasonic", deviceImei, new ContainerModificationMeasurementsUiModel()
        {
            Range = request.Range,
            Temperature = request.Temperature,
            Status = request.Status,
        }));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:UpdateContainerByDeviceImeiRoot -- Message:CONTAINER_UPDATE_WITH_DEVICE" +
              $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }


    /// <summary>
    /// Put - Update a container
    /// </summary>
    /// <param name="id">Container Id for modification</param>
    /// <param name="request">UpdateContainerResourceParameters for modification</param>
    /// <remarks>Update container </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateContainerRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateContainerAsync(long id, [FromBody] UpdateContainerResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new UpdateContainerCommand(id, userAudit.Model.Id, request));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:UpdateContainerRoot -- Message:CONTAINER_UPDATE_WITH_DEVICE" +
              $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing container - Soft Delete
    /// </summary>
    /// <param name="id">Container Id for deletion</param>
    /// <param name="request">DeleteContainerResourceParameters for deletion</param>
    /// <remarks>Delete existing container </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftContainerRoot")]
    [ValidateModel]
    public async Task<IActionResult> DeleteSoftContainerAsync(long id, [FromBody] DeleteContainerResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new DeleteSoftContainerCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:DeleteSoftContainerRoot -- Message:CONTAINER_UPDATE_WITH_DEVICE" +
              $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing container - Hard Delete
    /// </summary>
    /// <param name="id">Container Id for deletion</param>
    /// <remarks>Delete existing container </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardContainerRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardContainerAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!this.IsSuFromClaims())
        {
            Log.Error(
              $"--Method:DeleteHardContainerRoot -- Message:CONTAINERS_HARD_DELETION_NOT_SU" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardContainerAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }
        var response = await _mediator.Send(new DeleteHardContainerCommand(id));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:DeleteHardContainerRoot -- Message:CONTAINER_UPDATE_WITH_DEVICE" +
              $" -- Datetime:{DateTime.Now} -- Action: response.IsNull()");
            return NotFound("CONTAINER_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }
}// Class: ContainersController