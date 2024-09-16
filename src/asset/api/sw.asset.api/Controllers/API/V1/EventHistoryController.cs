using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.model.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;

namespace sw.asset.api.Controllers.API.V1;

/// <summary>
/// EventHistory Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class EventHistoryController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IServiceScopeFactory _scopeFactory;


    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="urlHelper"></param>
    /// <param name="typeHelperService"></param>
    /// <param name="propertyMappingService"></param>
    public EventHistoryController(
      IMediator mediator,
      IUrlHelper urlHelper,
      ITypeHelperService typeHelperService,
      IPropertyMappingService propertyMappingService, IServiceScopeFactory scopeFactory)
    {
        _mediator = mediator;
        _urlHelper = urlHelper;
        _typeHelperService = typeHelperService;
        _propertyMappingService = propertyMappingService;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Get - Fetch all event history By Device
    /// </summary>
    /// <param name="parameters">EventHistory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all event historys </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetEventHistoryRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetEventHistoryAsync(
      [FromQuery] GetEventHistoryResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryRoot -- Message:GetEventHistoryAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
        {
            Log.Error(
              $"--Method:GetEventHistoryRoot -- Message:GetEventHistoryAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: ValidMappingExistsFor");
            return BadRequest("EVENTS_HISTORY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
              (parameters.Fields))
        {
            Log.Error(
              $"--Method:GetEventHistoryRoot -- Message:GetEventHistoryAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: TypeHasProperties");
            return this.BadRequest("EVENTS_HISTORY_FIELDS_ERROR");
        }

        var fetchedEventsHistory = await _mediator.Send(new GetEventHistoryQuery(parameters));

        if (fetchedEventsHistory.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryRoot -- Message:EVENTS_HISTORY_NOT_FOUND" +
              $" -- Datetime:{DateTime.Now} -- Action: fetchedEventsHistory.IsNull()");
            return this.NotFound("EVENTS_HISTORY_NOT_FOUND");
        }

        if (!fetchedEventsHistory.IsSuccess())
        {
            Log.Error(
              $"--Method:GetEventHistoryRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: !fetchedEventsHistory.IsSuccess()");
            return OkOrBadRequest(fetchedEventsHistory.BrokenRules);
        }

        if (fetchedEventsHistory.Status == BusinessResultStatus.Fail)
        {
            Log.Error(
              $"--Method:GetEventHistoryRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: BusinessResultStatus.Fail");
            return OkOrNoResult(fetchedEventsHistory.BrokenRules);
        }

        var responseWithMetaData =
          CreateOkWithMetaData(fetchedEventsHistory.Model, mediaType, parameters, _urlHelper,
            "GetEventHistoryByIdAsync", "GetEventHistoriesAsync");
        Log.Information(
          $"--Method:GetEventHistoryRoot -- Message:GetEventHistoryAsync" +
          $" -- Datetime:{DateTime.Now} -- Action: CreateOkWithMetaData");

        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch event history by Container Id
    /// </summary>
    /// <param name="containerId">Container ID</param>
    /// <param name="parameters">EventHistory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all event historys </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("container/{containerId}", Name = "GetEventHistoryByContainerIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetEventHistoryByContainerIdAsync(long containerId,
      [FromQuery] GetEventHistoryResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryByDeviceRoot -- Message:GetEventHistoryByContainerIdRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
        {
            Log.Error(
              $"--Method:GetEventHistoryByContainerIdAsync -- Message:GetEventHistoryByContainerIdRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: ValidMappingExistsFor");
            return BadRequest("EVENTS_HISTORY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
              (parameters.Fields))
        {
            Log.Error(
              $"--Method:GetEventHistoryByContainerIdAsync -- Message:GetEventHistoryByContainerIdRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: TypeHasProperties");
            return this.BadRequest("EVENTS_HISTORY_FIELDS_ERROR");
        }

        var fetchedEventsHistory = await _mediator.Send(new GetEventHistoryByContainerIdQuery(containerId, parameters));

        if (fetchedEventsHistory.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryByContainerIdRoot -- Message:EVENTS_HISTORY_NOT_FOUND" +
              $" -- Datetime:{DateTime.Now} -- Action: fetchedEventsHistory.IsNull()");
            return this.NotFound("EVENTS_HISTORY_NOT_FOUND");
        }

        if (!fetchedEventsHistory.IsSuccess())
        {
            Log.Error(
              $"--Method:GetEventHistoryByContainerIdRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: !fetchedEventsHistory.IsSuccess()");
            return OkOrBadRequest(fetchedEventsHistory.BrokenRules);
        }

        if (fetchedEventsHistory.Status == BusinessResultStatus.Fail)
        {
            Log.Error(
              $"--Method:GetEventHistoryByContainerIdRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: BusinessResultStatus.Fail");
            return OkOrNoResult(fetchedEventsHistory.BrokenRules);
        }

        var responseWithMetaData =
          CreateOkWithMetaData(fetchedEventsHistory.Model, mediaType, parameters, _urlHelper,
            "GetEventHistoryByContainerIdRoot", "GetEventHistoryByContainerIdAsync");
        Log.Information(
          $"--Method:GetEventHistoryByContainerIdRoot -- Message:GetEventHistoryByContainerIdRoot" +
          $" -- Datetime:{DateTime.Now} -- Action: CreateOkWithMetaData");

        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch N records of event history
    /// </summary>
    /// <param name="parameters">EventHistory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <param name="numberOfRecords"></param>
    /// <remarks>Fetch all event histories </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("records/{numberOfRecords}", Name = "GetEventHistoryNLastRecordsRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetEventHistoryNLastRecordsAsync(
      [FromQuery] GetEventHistoryResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType, int numberOfRecords = 10)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryNLastRecordsAsync -- Message:GetEventHistoryNLastRecordsRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
        {
            Log.Error(
              $"--Method:GetEventHistoryNLastRecordsAsync -- Message:GetEventHistoryNLastRecordsRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: ValidMappingExistsFor");
            return BadRequest("EVENTS_HISTORY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
              (parameters.Fields))
        {
            Log.Error(
              $"--Method:GetEventHistoryNLastRecordsAsync -- Message:GetEventHistoryNLastRecordsRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: TypeHasProperties");
            return this.BadRequest("EVENTS_HISTORY_FIELDS_ERROR");
        }

        var fetchedEventsHistory = await _mediator.Send(new GetEventHistoryNLastRecordsQuery(n, parameters));

        if (fetchedEventsHistory.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryNLastRecordsAsync -- Message:EVENTS_HISTORY_NOT_FOUND" +
              $" -- Datetime:{DateTime.Now} -- Action: fetchedEventsHistory.IsNull()");
            return this.NotFound("EVENTS_HISTORY_NOT_FOUND");
        }

        if (!fetchedEventsHistory.IsSuccess())
        {
            Log.Error(
              $"--Method:GetEventHistoryNLastRecordsAsync -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: !fetchedEventsHistory.IsSuccess()");
            return OkOrBadRequest(fetchedEventsHistory.BrokenRules);
        }

        if (fetchedEventsHistory.Status == BusinessResultStatus.Fail)
        {
            Log.Error(
              $"--Method:GetEventHistoryNLastRecordsAsync -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: BusinessResultStatus.Fail");
            return OkOrNoResult(fetchedEventsHistory.BrokenRules);
        }

        var responseWithMetaData =
          CreateOkWithMetaData(fetchedEventsHistory.Model, mediaType, parameters, _urlHelper,
            "GetEventHistoryNLastRecordsAsync", "GetEventHistoryNLastRecordsAsync");
        Log.Information(
          $"--Method:GetEventHistoryNLastRecordsAsync -- Message:GetEventHistoryNLastRecordsRoot" +
          $" -- Datetime:{DateTime.Now} -- Action: CreateOkWithMetaData");

        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch all event history Between Dates for specific Container
    /// </summary>
    /// <param name="end">End Date</param>
    /// <param name="parameters">EventHistory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <param name="containerId">Container Id</param>
    /// <param name="start">Start Date</param>
    /// <remarks>Fetch all event historys </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("container/{containerId}/start_date/{start}/end_date/{end}", Name = "GetEventHistoryBetweenDatesRofContainerRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetEventHistoryBetweenDatesForContainerAsync(long containerId, DateTime start, DateTime end,
      [FromQuery] GetEventHistoryResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRofContainerRoot -- Message:GetEventHistoryBetweenDatesForContainerAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRofContainerRoot -- Message:GetEventHistoryBetweenDatesForContainerAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: ValidMappingExistsFor");
            return BadRequest("EVENTS_HISTORY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
              (parameters.Fields))
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRofContainerRoot -- Message:GetEventHistoryBetweenDatesForContainerAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: TypeHasProperties");
            return this.BadRequest("EVENTS_HISTORY_FIELDS_ERROR");
        }

        var fetchedEventsHistory = await _mediator.Send(new GetEventHistoryBetweenDatesForContainerQuery(containerId, start, end, parameters));

        if (fetchedEventsHistory.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRofContainerRoot -- Message:EVENTS_HISTORY_NOT_FOUND" +
              $" -- Datetime:{DateTime.Now} -- Action: fetchedEventsHistory.IsNull()");
            return this.NotFound("EVENTS_HISTORY_NOT_FOUND");
        }

        if (!fetchedEventsHistory.IsSuccess())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRofContainerRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: !fetchedEventsHistory.IsSuccess()");
            return OkOrBadRequest(fetchedEventsHistory.BrokenRules);
        }

        if (fetchedEventsHistory.Status == BusinessResultStatus.Fail)
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRofContainerRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: BusinessResultStatus.Fail");
            return OkOrNoResult(fetchedEventsHistory.BrokenRules);
        }

        var responseWithMetaData =
          CreateOkWithMetaData(fetchedEventsHistory.Model, mediaType, parameters, _urlHelper,
            "GetEventHistoryBetweenDatesForContainerAsync", "GetEventHistoryBetweenDatesForContainerAsync");
        Log.Information(
          $"--Method:GetEventHistoryBetweenDatesRofContainerRoot -- Message:GetEventHistoryBetweenDatesForContainerAsync" +
          $" -- Datetime:{DateTime.Now} -- Action: CreateOkWithMetaData");

        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch all event history
    /// </summary>
    /// <param name="deviceImei">Device Imei</param>
    /// <param name="parameters">EventHistory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all event historys </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("devices/{deviceImei}", Name = "GetEventHistoryByDeviceRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetEventHistoryByDeviceAsync(string deviceImei,
      [FromQuery] GetEventHistoryResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryByDeviceRoot -- Message:GetEventHistoryByDeviceRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
        {
            Log.Error(
              $"--Method:GetEventHistoryByDeviceRoot -- Message:GetEventHistoryByDeviceRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: ValidMappingExistsFor");
            return BadRequest("EVENTS_HISTORY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
              (parameters.Fields))
        {
            Log.Error(
              $"--Method:GetEventHistoryByDeviceRoot -- Message:GetEventHistoryByDeviceRoot" +
              $" -- Datetime:{DateTime.Now} -- Action: TypeHasProperties");
            return this.BadRequest("EVENTS_HISTORY_FIELDS_ERROR");
        }

        var fetchedEventsHistory = await _mediator.Send(new GetEventHistoryByDeviceQuery(deviceImei, parameters));

        if (fetchedEventsHistory.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryByDeviceRoot -- Message:EVENTS_HISTORY_NOT_FOUND" +
              $" -- Datetime:{DateTime.Now} -- Action: fetchedEventsHistory.IsNull()");
            return this.NotFound("EVENTS_HISTORY_NOT_FOUND");
        }

        if (!fetchedEventsHistory.IsSuccess())
        {
            Log.Error(
              $"--Method:GetEventHistoryByDeviceRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: !fetchedEventsHistory.IsSuccess()");
            return OkOrBadRequest(fetchedEventsHistory.BrokenRules);
        }

        if (fetchedEventsHistory.Status == BusinessResultStatus.Fail)
        {
            Log.Error(
              $"--Method:GetEventHistoryByDeviceRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: BusinessResultStatus.Fail");
            return OkOrNoResult(fetchedEventsHistory.BrokenRules);
        }

        var responseWithMetaData =
          CreateOkWithMetaData(fetchedEventsHistory.Model, mediaType, parameters, _urlHelper,
            "GetEventHistoryByDeviceRoot", "GetEventHistoriesAsync");
        Log.Information(
          $"--Method:GetEventHistoryByDeviceRoot -- Message:GetEventHistoryByDeviceRoot" +
          $" -- Datetime:{DateTime.Now} -- Action: CreateOkWithMetaData");

        return Ok(responseWithMetaData);
    }


    /// <summary>
    /// Get - Fetch all event history Between Dates
    /// </summary>
    /// <param name="end">End Date</param>
    /// <param name="parameters">EventHistory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <param name="start">Start Date</param>
    /// <remarks>Fetch all event historys </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("start_date/{start}/end_date/{end}", Name = "GetEventHistoryBetweenDatesRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetEventHistoryBetweenDatesAsync(DateTime start, DateTime end,
      [FromQuery] GetEventHistoryResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRoot -- Message:GetEventHistoryBetweenDatesAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRoot -- Message:GetEventHistoryBetweenDatesAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: ValidMappingExistsFor");
            return BadRequest("EVENTS_HISTORY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
              (parameters.Fields))
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRoot -- Message:GetEventHistoryBetweenDatesAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: TypeHasProperties");
            return this.BadRequest("EVENTS_HISTORY_FIELDS_ERROR");
        }

        var fetchedEventsHistory = await _mediator.Send(new GetEventHistoryBetweenDatesQuery(start, end, parameters));

        if (fetchedEventsHistory.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRoot -- Message:EVENTS_HISTORY_NOT_FOUND" +
              $" -- Datetime:{DateTime.Now} -- Action: fetchedEventsHistory.IsNull()");
            return this.NotFound("EVENTS_HISTORY_NOT_FOUND");
        }

        if (!fetchedEventsHistory.IsSuccess())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: !fetchedEventsHistory.IsSuccess()");
            return OkOrBadRequest(fetchedEventsHistory.BrokenRules);
        }

        if (fetchedEventsHistory.Status == BusinessResultStatus.Fail)
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: BusinessResultStatus.Fail");
            return OkOrNoResult(fetchedEventsHistory.BrokenRules);
        }

        var responseWithMetaData =
          CreateOkWithMetaData(fetchedEventsHistory.Model, mediaType, parameters, _urlHelper,
            "GetEventHistoryBetweenDatesAsync", "GetEventHistoryBetweenDatesAsync");
        Log.Information(
          $"--Method:GetEventHistoryBetweenDatesRoot -- Message:GetEventHistoryBetweenDatesAsync" +
          $" -- Datetime:{DateTime.Now} -- Action: CreateOkWithMetaData");

        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch all event history Between Dates
    /// </summary>
    /// <param name="end">End Date</param>
    /// <param name="parameters">EventHistory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <param name="deviceImei">Device Imei</param>
    /// <param name="start">Start Date</param>
    /// <remarks>Fetch all event historys </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("devices/{deviceImei}/start_date/{start}/end_date/{end}", Name = "GetEventHistoryBetweenDatesForDeviceRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetEventHistoryBetweenDatesForDeviceAsync(string deviceImei, DateTime start, DateTime end,
      [FromQuery] GetEventHistoryResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesForDeviceRoot -- Message:GetEventHistoryBetweenDatesForDeviceAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<EventHistoryUiModel, EventHistory>(parameters.OrderBy))
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesForDeviceRoot -- Message:GetEventHistoryBetweenDatesForDeviceAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: ValidMappingExistsFor");
            return BadRequest("EVENTS_HISTORY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<EventHistoryUiModel>
              (parameters.Fields))
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesForDeviceRoot -- Message:GetEventHistoryBetweenDatesForDeviceAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: TypeHasProperties");
            return this.BadRequest("EVENTS_HISTORY_FIELDS_ERROR");
        }

        var fetchedEventsHistory = await _mediator.Send(new GetEventHistoryBetweenDatesForDeviceQuery(deviceImei, start, end, parameters));

        if (fetchedEventsHistory.IsNull())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesForDeviceRoot -- Message:EVENTS_HISTORY_NOT_FOUND" +
              $" -- Datetime:{DateTime.Now} -- Action: fetchedEventsHistory.IsNull()");
            return this.NotFound("EVENTS_HISTORY_NOT_FOUND");
        }

        if (!fetchedEventsHistory.IsSuccess())
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesForDeviceRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: !fetchedEventsHistory.IsSuccess()");
            return OkOrBadRequest(fetchedEventsHistory.BrokenRules);
        }

        if (fetchedEventsHistory.Status == BusinessResultStatus.Fail)
        {
            Log.Error(
              $"--Method:GetEventHistoryBetweenDatesForDeviceRoot -- Message:EVENT_HISTORY_IS_NOT_SUCCESS" +
              $" -- Datetime:{DateTime.Now} -- Action: BusinessResultStatus.Fail");
            return OkOrNoResult(fetchedEventsHistory.BrokenRules);
        }

        var responseWithMetaData =
          CreateOkWithMetaData(fetchedEventsHistory.Model, mediaType, parameters, _urlHelper,
            "GetEventHistoryBetweenDatesForDeviceAsync", "GetEventHistoryBetweenDatesForDeviceAsync");
        Log.Information(
          $"--Method:GetEventHistoryBetweenDatesForDeviceRoot -- Message:GetEventHistoryBetweenDatesForDeviceAsync" +
          $" -- Datetime:{DateTime.Now} -- Action: CreateOkWithMetaData");

        return Ok(responseWithMetaData);
    }

}// Class: EventHistoryController