using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.ResourceParameters.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.model.SensorTypes;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
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
/// SensorType Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class SensorTypesController : BaseController
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
    public SensorTypesController(
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
    /// Get - Fetch all sensor types
    /// </summary>
    /// <param name="parameters">SensorType parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all sensor types </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetSensorTypesRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetSensorTypesAsync(
      [FromQuery] GetSensorTypesResourceParameters parameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<SensorTypeUiModel, SensorType>(parameters.OrderBy))
        {
            return BadRequest("SENSOR_TYPES_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<SensorTypeUiModel>
              (parameters.Fields))
        {
            return BadRequest("SENSOR_TYPES_FIELDS_ERROR");
        }

        var response = await _mediator.Send(new GetSensorTypesQuery(parameters));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:GetSensorTypesRoot -- Message:SENSORS_TYPES_FETCH" +
                  $" -- Datetime:{DateTime.Now} -- Action: GetSensorTypesAsync");
            return NotFound("ERROR_FETCH_SENSOR_TYPES");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetSensorTypeByIdAsync", "GetSensorTypesAsync");
        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one sensor type
    /// </summary>
    /// <param name="id">SensorType Id for fetching</param>
    /// <remarks>Fetch one sensor type </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetSensorTypeByIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetSensorTypeByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }
        var response = await _mediator.Send(new GetSensorTypeByIdQuery(id));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:GetSensorTypeByIdRoot -- Message:SENSOR_TYPE_FETCH" +
                  $" -- Datetime:{DateTime.Now} -- Action: GetSensorTypeByIdAsync");
            return NotFound("SENSOR_TYPE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Post - Create a sensor type
    /// </summary>
    /// <param name="request">CreateSensorTypeResourceParameters for creation</param>
    /// <remarks>Create sensor type </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostSensorTypeRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostSensorTypeAsync([FromBody] CreateSensorTypeResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }
        var response = await _mediator.Send(
          new CreateSensorTypeCommand(userAudit.Model.Id, request)
        );

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:PostSensorTypeRoot -- Message:SENSOR_TYPE_CREATION" +
                  $" -- Datetime:{DateTime.Now} -- Action: PostSensorTypeAsync");
            return NotFound("SENSOR_TYPE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Put - Update a sensor type
    /// </summary>
    /// <param name="id">SensorType Id for modification</param>
    /// <param name="request">UpdateSensorTypeResourceParameters for modification</param>
    /// <remarks>Update sensor type </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateSensorTypeRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateSensorTypeAsync(long id, [FromBody] UpdateSensorTypeResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }
        var response = await _mediator.Send(new UpdateSensorTypeCommand(userAudit.Model.Id, id, request));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:UpdateSensorTypeRoot -- Message:SENSOR_TYPE_UPDATE" +
                  $" -- Datetime:{DateTime.Now} -- Action: UpdateSensorTypeAsync");
            return NotFound("SENSOR_TYPE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing sensor type - Soft Delete
    /// </summary>
    /// <param name="id">SensorType Id for deletion</param>
    /// <param name="request">DeleteSensorTypeResourceParameters for deletion</param>
    /// <remarks>Delete existing sensor type </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftSensorTypeRoot")]
    [ValidateModel]
    public async Task<IActionResult> DeleteSoftSensorTypeAsync(long id, [FromBody] DeleteSensorTypeResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        var response = await _mediator.Send(new DeleteSoftSensorTypeCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:DeleteSoftSensorTypeRoot -- Message:SENSOR_TYPE_SOFT_DELETION" +
                  $" -- Datetime:{DateTime.Now} -- Action: DeleteSoftSensorTypeAsync");
            return NotFound("SENSOR_TYPE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing sensor type - Hard Delete
    /// </summary>
    /// <param name="id">SensorType Id for deletion</param>
    /// <remarks>Delete existing sensor type </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardSensorTypeRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardSensorTypeAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }
        if (!this.IsSuFromClaims())
        {
            Log.Error(
                $"--Method:DeleteHardSensorTypeRoot -- Message:SENSORTYPES_HARD_DELETION" +
                $" -- Datetime:{DateTime.Now} -- Action: DeleteHardSensorTypeAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }
        var response = await _mediator.Send(new DeleteHardSensorTypeCommand(id));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:DeleteHardSensorTypeRoot -- Message:SENSOR_TYPE_HARD_DELETION" +
                  $" -- Datetime:{DateTime.Now} -- Action: DeleteHardSensorTypeAsync");
            return NotFound("SENSOR_TYPE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }
}