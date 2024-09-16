using System;
using System.Threading.Tasks;
using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.model.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace sw.asset.api.Controllers.API.V1;

/// <summary>
/// DeviceModel Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class DeviceModelsController : BaseController
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
    public DeviceModelsController(
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
    /// Get - Fetch all device models
    /// </summary>
    /// <param name="parameters">DeviceModel parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all device models </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetDeviceModelsAsync")]
    [ValidateModel]
    public async Task<IActionResult> GetDeviceModelsAsync(
        [FromQuery] GetDeviceModelsResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
        && !_propertyMappingService.ValidMappingExistsFor<DeviceModelUiModel, DeviceModel>(parameters.OrderBy))
        {
            return BadRequest("DEVICEMODEL_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<DeviceModelUiModel>
            (parameters.Fields))
        {
            return BadRequest("DEVICEMODEL_FIELDS_ERROR");
        }

        var response = await _mediator.Send(new GetDeviceModelsQuery(parameters));

        if (response.IsNull())
        {
            return NotFound("DEVICEMODEL_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetDeviceModelByIdAsync", "GetDeviceModelsAsync");
        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one device model
    /// </summary>
    /// <param name="id">DeviceModel Id for fetching</param>
    /// <remarks>Fetch one device model </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetDeviceModelByIdAsync")]
    [ValidateModel]
    public async Task<IActionResult> GetDeviceModelByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new GetDeviceModelByIdQuery(id));

        if (response.IsNull())
        {
            return NotFound("DEVICEMODEL_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Post - Create a device model
    /// </summary>
    /// <param name="request">CreateDeviceModelResourceParameters for creation</param>
    /// <remarks>Create device model </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostDeviceModelAsync")]
    [ValidateModel]
    public async Task<IActionResult> PostDeviceModelAsync([FromBody] CreateDeviceModelResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(
            new CreateDeviceModelCommand(userAudit.Model.Id, request.Name, request.CodeErp, request.CodeName, request.Enabled)
        );

        if (response.IsNull())
        {
            return NotFound("DEVICEMODEL_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Put - Update a device model
    /// </summary>
    /// <param name="id">DeviceModel Id for modification</param>
    /// <param name="request">UpdateDeviceModelResourceParameters for modification</param>
    /// <remarks>Update device model </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateDeviceModelAsync")]
    [ValidateModel]
    public async Task<IActionResult> UpdateDeviceModelAsync(long id, [FromBody] UpdateDeviceModelResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new UpdateDeviceModelCommand(userAudit.Model.Id, id, request.Name, request.CodeErp, request.CodeName, request.Enabled));

        if (response.IsNull())
        {
            return NotFound("DEVICEMODEL_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing device model - Soft Delete
    /// </summary>
    /// <param name="id">DeviceModel Id for deletion</param>
    /// <param name="request">DeleteDeviceModelResourceParameters for deletion</param>
    /// <remarks>Delete existing device model </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftDeviceModelAsync")]
    [ValidateModel]
    public async Task<IActionResult> DeleteSoftDeviceModelAsync(long id, [FromBody] DeleteDeviceModelResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new DeleteSoftDeviceModelCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            return NotFound("DEVICEMODEL_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing device model - Hard Delete
    /// </summary>
    /// <param name="id">DeviceModel Id for deletion</param>
    /// <remarks>Delete existing device model </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardDeviceModelRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardDeviceModelAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!this.IsSuFromClaims())
        {
            Log.Error(
              $"--Method:DeleteHardDeviceModelRoot -- Message:DEVICEMODELS_HARD_DELETION_NOT_SU" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardDeviceModelAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }
        var response = await _mediator.Send(new DeleteHardDeviceModelCommand(id));

        if (response.IsNull())
        {
            return NotFound("DEVICEMODEL_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }
}//Class: DeviceModelsController
