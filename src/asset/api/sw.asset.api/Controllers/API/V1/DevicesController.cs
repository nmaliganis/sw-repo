using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.asset.model.Devices;
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
using System.Threading.Tasks;

namespace sw.asset.api.Controllers.API.V1;

/// <summary>
/// Device Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]

public class DevicesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;

    private readonly ICreateDeviceProcessor _createDeviceProcessor;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="urlHelper"></param>
    /// <param name="typeHelperService"></param>
    /// <param name="propertyMappingService"></param>
    /// <param name="createDeviceProcessor"></param>
    public DevicesController(
      IMediator mediator,
      IUrlHelper urlHelper,
      ITypeHelperService typeHelperService,
      IPropertyMappingService propertyMappingService,
      ICreateDeviceProcessor createDeviceProcessor)
    {
        _mediator = mediator;
        _urlHelper = urlHelper;
        _typeHelperService = typeHelperService;
        _propertyMappingService = propertyMappingService;
        _createDeviceProcessor = createDeviceProcessor;
    }

    /// <summary>
    /// Get - Fetch all devices
    /// </summary>
    /// <param name="parameters">Device parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all devices</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetDevicesRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetDevicesAsync(
      [FromQuery] GetDevicesResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy)
                && !this._propertyMappingService.ValidMappingExistsFor<DeviceUiModel, Device>(parameters.OrderBy))
        {
            return this.BadRequest("DEVICES_MODEL_ERROR");
        }

        if (!this._typeHelperService.TypeHasProperties<DeviceUiModel>
                (parameters.Fields))
        {
            return this.BadRequest("DEVICES_FIELDS_ERROR");
        }

        BusinessResult<PagedList<DeviceUiModel>> fetchedDevices;
        if (this.IsSuFromClaims())
        {
            try
            {
                Log.Information(
                    $"--Method:GetDevicesAsync -- Message:DEVICES_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetDevicesAsync");
                fetchedDevices = await this._mediator.Send(new GetDevicesQuery(parameters));
                Log.Information(
                    $"--Method:GetDevicesAsync -- Message:DEVICES_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just After : GetDevicesAsync");
            }
            catch (Exception e)
            {
                Log.Error(
                    $"--Method:GetDevicesAsync -- Message:DEVICES_FETCH_ERROR" +
                    $" -- Datetime:{DateTime.UtcNow} -- DeviceInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                return this.BadRequest("ERROR_FETCH_DEVICES");
            }
        }
        else
        {
            try
            {
                Log.Information(
                    $"--Method:GetDevicesAsync -- Message:DEVICES_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetDevicesAsync");
                //Fetch With Company ?
                fetchedDevices = await this._mediator.Send(new GetDevicesQuery(parameters));
                Log.Information(
                    $"--Method:GetDevicesAsync -- Message:DEVICES_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just After : GetDevicesAsync");
            }
            catch (Exception e)
            {
                Log.Error(
                    $"--Method:GetDevicesAsync -- Message:DEVICES_FETCH_ERROR" +
                    $" -- Datetime:{DateTime.UtcNow} -- DeviceInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                return this.BadRequest("ERROR_FETCH_DEVICES");
            }
        }

        if (fetchedDevices.IsNull())
        {
            return this.NotFound("DEVICES_NOT_FOUND");
        }

        if (!fetchedDevices.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedDevices.BrokenRules);
        }

        if (fetchedDevices.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedDevices.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedDevices.Model, mediaType,
            parameters, this._urlHelper, "GetDeviceByIdAsync", "GetDevicesAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one device
    /// </summary>
    /// <param name="id">Device Id for fetching</param>
    /// <remarks>Fetch one device </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetDeviceByIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetDeviceByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetDeviceByIdRoot -- Message:DEVICE_FETCH" +
              $" -- Datetime:{DateTime.Now} - Details: USER_AUDIT_NOT_FOUND -- Action: GetDeviceByIdAsync");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var fetchedDevice = await this._mediator.Send(new GetDeviceByIdQuery(id));

        if (fetchedDevice.IsNull())
        {
            Log.Error(
              $"--Method:GetDeviceByIdRoot -- Message:DEVICE_FETCH" +
              $" -- Datetime:{DateTime.Now} -- Action: GetDeviceByIdAsync");
            return this.NotFound("DEVICE_NOT_FOUND");
        }

        if (!fetchedDevice.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedDevice.BrokenRules);
        }

        if (fetchedDevice.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedDevice.BrokenRules);
        }

        return this.Ok(fetchedDevice);
    }

    /// <summary>
    /// Post - Create a device
    /// </summary>
    /// <param name="request">CreateDeviceResourceParameters for creation</param>
    /// <remarks>Create device </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostDeviceRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostDeviceAsync([FromBody] CreateDeviceResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:PostDeviceRoot -- Message:PostDeviceAsync" +
              $" -- Datetime:{DateTime.Now} -- Action: PostDeviceAsync");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        BusinessResult<DeviceUiModel> createdDevice = await this._mediator.Send(new CreateDeviceCommand(userAudit.Model.Id, request));

        if (createdDevice.IsNull())
        {
            Log.Error(
              $"--Method:PostDeviceRoot -- Message:DEVICES_CREATION" +
              $" -- Datetime:{DateTime.Now} -- Action: PostDeviceAsync");
            return this.NotFound("ERROR_DEVICE_CREATED_NOT_FOUND");
        }

        if (!createdDevice.IsSuccess())
        {
            return this.OkOrBadRequest(createdDevice.BrokenRules);
        }

        if (createdDevice.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(createdDevice.BrokenRules);
        }

        return this.CreatedOrNoResult(createdDevice);
    }

    /// <summary>
    /// Put - Update a device
    /// </summary>
    /// <param name="id">Device Id for modification</param>
    /// <param name="request">UpdateDeviceResourceParameters for modification</param>
    /// <remarks>Update device </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateDeviceRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateDeviceAsync(long id, [FromBody] UpdateDeviceResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await this._mediator.Send(new UpdateDeviceCommand(id, userAudit.Model.Id, request));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:UpdateDeviceRoot -- Message:DEVICES_UPDATE" +
              $" -- Datetime:{DateTime.Now} -- Action: UpdateDeviceAsync");
            return this.NotFound("DEVICE_NOT_FOUND");
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
    /// Delete - Delete an existing device - Soft Delete
    /// </summary>
    /// <param name="id">Device Id for deletion</param>
    /// <param name="request">DeleteDeviceResourceParameters for deletion</param>
    /// <remarks>Delete existing device </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftDeviceRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> DeleteSoftDeviceAsync(long id, [FromBody] DeleteDeviceResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        var response = await this._mediator.Send(new DeleteSoftDeviceCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:DeleteSoftDeviceRoot -- Message:DEVICES_SOFT_DELETION" +
              $" -- Datetime:{DateTime.Now} -- Action: DeleteSoftDeviceAsync");
            return this.NotFound("DEVICE_NOT_FOUND");
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
    /// Delete - Delete an existing device - Hard Delete
    /// </summary>
    /// <param name="id">Device Id for deletion</param>
    /// <remarks>Delete existing device </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardDeviceRoot")]
    [ValidateModel]
    //[Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardDeviceAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        if (!this.IsSuFromClaims())
        {
            Log.Error(
              $"--Method:DeleteHardDeviceRoot -- Message:DEVICES_HARD_DELETION_NOT_SU" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardDeviceAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }

        var response = await this._mediator.Send(new DeleteHardDeviceCommand(id));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:DeleteHardDeviceRoot -- Message:DEVICES_HARD_DELETION" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardDeviceAsync");
            return this.NotFound("DEVICE_NOT_FOUND");
        }

        if (!response.IsSuccess())
        {
            Log.Error(
              $"--Method:DeleteHardDeviceRoot -- Message:DEVICES_HARD_DELETION" +
              $" -- Datetime:{DateTime.UtcNow} -- Details:{response.BrokenRules} -- Action: DeleteHardDeviceAsync");
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Ok(response);
    }
}//Class: DevicesController