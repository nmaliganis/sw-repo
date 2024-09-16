using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.ResourceParameters.Sensor;
using sw.asset.common.dtos.ResourceParameters.Sensors;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.model.Sensors;
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
/// Sensor Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class SensorsController : BaseController
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
    public SensorsController(
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
    /// Get - Fetch all sensors
    /// </summary>
    /// <param name="parameters">Sensor parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all sensors </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetSensorsRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetSensorsAsync(
      [FromQuery] GetSensorsResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<SensorUiModel, Sensor>(parameters.OrderBy))
        {
            return this.BadRequest("SENSORS_MODEL_ERROR");
        }

        if (!this._typeHelperService.TypeHasProperties<SensorUiModel>
              (parameters.Fields))
        {
            return this.BadRequest("SENSORS_FIELDS_ERROR");
        }

        BusinessResult<PagedList<SensorUiModel>> fetchedSensors;
        if (this.IsSuFromClaims())
        {
            try
            {
                Log.Information(
                  $"--Method:GetSensorsRoot -- Message:SENSORS_FETCH" +
                  $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetSensorsAsync");
                fetchedSensors = await this._mediator.Send(new GetSensorsQuery(parameters));
                Log.Information(
                  $"--Method:GetSensorsRoot -- Message:SENSORS_FETCH" +
                  $" -- Datetime:{DateTime.UtcNow} -- Just After : GetSensorsAsync");
            }
            catch (Exception e)
            {
                Log.Error(
                  $"--Method:GetSensorsRoot -- Message:SENSORS_FETCH_ERROR" +
                  $" -- Datetime:{DateTime.UtcNow} -- SensorInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                return this.BadRequest("ERROR_FETCH_SENSORS");
            }
        }
        else
        {
            try
            {
                Log.Information(
                  $"--Method:GetSensorsRoot -- Message:SENSORS_FETCH" +
                  $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetSensorsAsync");
                //Fetch With Company ?
                fetchedSensors = await this._mediator.Send(new GetSensorsQuery(parameters));
                Log.Information(
                  $"--Method:GetSensorsRoot -- Message:SENSORS_FETCH" +
                  $" -- Datetime:{DateTime.UtcNow} -- Just After : GetSensorsAsync");
            }
            catch (Exception e)
            {
                Log.Error(
                  $"--Method:GetSensorsRoot -- Message:SENSORS_FETCH_ERROR" +
                  $" -- Datetime:{DateTime.UtcNow} -- SensorInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                return this.BadRequest("ERROR_FETCH_SENSORS");
            }
        }

        if (fetchedSensors.IsNull())
        {
            return this.NotFound("SENSORS_NOT_FOUND");
        }

        if (!fetchedSensors.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedSensors.BrokenRules);
        }

        if (fetchedSensors.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedSensors.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedSensors.Model, mediaType,
          parameters, this._urlHelper, "GetSensorByIdAsync", "GetSensorsAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one sensor
    /// </summary>
    /// <param name="id">Sensor Id for fetching</param>
    /// <remarks>Fetch one sensor </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetSensorByIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetSensorByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            Log.Error(
              $"--Method:GetSensorByIdRoot -- Message:SENSORS_FETCH_BY_ID" +
              $" -- Datetime:{DateTime.Now} - Details: USER_AUDIT_NOT_FOUND -- Action: GetSensorByIdAsync");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var fetchedSensor = await this._mediator.Send(new GetSensorByIdQuery(id));

        if (fetchedSensor.IsNull())
        {
            Log.Error(
              $"--Method:GetSensorByIdRoot -- Message:SENSORS_FETCH_BY_ID" +
              $" -- Datetime:{DateTime.Now} -- Action: GetSensorByIdAsync");
            return this.NotFound("SENSOR_NOT_FOUND");
        }

        if (!fetchedSensor.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedSensor.BrokenRules);
        }

        if (fetchedSensor.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedSensor.BrokenRules);
        }

        return this.Ok(fetchedSensor);
    }

    /// <summary>
    /// Post - Create a sensor
    /// </summary>
    /// <param name="request">CreateSensorResourceParameters for creation</param>
    /// <remarks>Create sensor </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostSensorRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostSensorAsync([FromBody] CreateSensorResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        BusinessResult<SensorUiModel> createdSensor = await this._mediator.Send(new CreateSensorCommand(userAudit.Model.Id, request));


        if (createdSensor.IsNull())
        {
            Log.Error(
              $"--Method:PostSensorRoot -- Message:SENSORS_CREATION" +
              $" -- Datetime:{DateTime.Now} -- Action: PostSensorAsync");
            return this.NotFound("ERROR_SENSOR_CREATED_NOT_FOUND");
        }

        if (!createdSensor.IsSuccess())
        {
            return this.OkOrBadRequest(createdSensor.BrokenRules);
        }

        if (createdSensor.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(createdSensor.BrokenRules);
        }

        return this.CreatedOrNoResult(createdSensor);
    }

    /// <summary>
    /// Post - Create a sensor
    /// </summary>
    /// <param name="imei">Device Imei to Create Sensor</param>
    /// <param name="request">CreateSensorByImeiResourceParameters for creation</param>
    /// <remarks>Create sensor </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost("devices/{imei}", Name = "PostSensorByDeviceImeiRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostSensorByImeiAsync(string imei, [FromBody] CreateSensorByImeiResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        BusinessResult<SensorUiModel> createdSensor = await this._mediator.Send(new CreateSensorByDeviceImeiCommand(userAudit.Model.Id, imei, request));


        if (createdSensor.IsNull())
        {
            Log.Error(
              $"--Method:PostSensorByDeviceImeiRoot -- Message:SENSORS_CREATION" +
              $" -- Datetime:{DateTime.Now} -- Action: PostSensorByImeiAsync");
            return this.NotFound("ERROR_SENSOR_CREATED_NOT_FOUND");
        }

        if (!createdSensor.IsSuccess())
        {
            return this.OkOrBadRequest(createdSensor.BrokenRules);
        }

        if (createdSensor.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(createdSensor.BrokenRules);
        }

        return this.CreatedOrNoResult(createdSensor);
    }

    /// <summary>
    /// Put - Update a sensor
    /// </summary>
    /// <param name="id">Sensor Id for modification</param>
    /// <param name="request">UpdateSensorResourceParameters for modification</param>
    /// <remarks>Update sensor </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateSensorRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateSensorAsync(long id, [FromBody] UpdateSensorResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await this._mediator.Send(new UpdateSensorCommand(id, userAudit.Model.Id, request));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:UpdateSensorRoot -- Message:SENSORS_UPDATE" +
              $" -- Datetime:{DateTime.Now} -- Action: UpdateSensorAsync");
            return this.NotFound("SENSOR_NOT_FOUND");
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
    /// Delete - Delete an existing sensor - Soft Delete
    /// </summary>
    /// <param name="id">Sensor Id for deletion</param>
    /// <param name="request">DeleteSensorResourceParameters for deletion</param>
    /// <remarks>Delete existing sensor </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftSensorRoot")]
    [ValidateModel]
    public async Task<IActionResult> DeleteSoftSensorAsync(long id, [FromBody] DeleteSensorResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        var response = await this._mediator.Send(new DeleteSoftSensorCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:DeleteSoftSensorRoot -- Message:SENSORS_SOFT_DELETION" +
              $" -- Datetime:{DateTime.Now} -- Action: DeleteSoftSensorAsync");
            return this.NotFound("SENSOR_NOT_FOUND");
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
    /// Delete - Delete an existing sensor - Hard Delete
    /// </summary>
    /// <param name="id">Sensor Id for deletion</param>
    /// <remarks>Delete existing sensor </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardSensorRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardSensorAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        if (!this.IsSuFromClaims())
        {
            Log.Error(
              $"--Method:DeleteHardSensorRoot -- Message:SENSORS_HARD_DELETION_NOT_SU" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardSensorAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }

        var response = await this._mediator.Send(new DeleteHardSensorCommand(id));

        if (response.IsNull())
        {
            Log.Error(
              $"--Method:DeleteHardSensorRoot -- Message:SENSORS_HARD_DELETION" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardSensorAsync");
            return this.NotFound("SENSOR_NOT_FOUND");
        }

        if (!response.IsSuccess())
        {
            Log.Error(
              $"--Method:DeleteHardSensorRoot -- Message:SENSORS_HARD_DELETION" +
              $" -- Datetime:{DateTime.UtcNow} -- Details:{response.BrokenRules} -- Action: DeleteHardSensorAsync");
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Ok(response);
    }
}