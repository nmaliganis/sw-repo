using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.model.Assets.Vehicles;
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
/// Vehicle Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class VehiclesController : BaseController
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
    public VehiclesController(
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
    /// Get - Fetch all vehicles
    /// </summary>
    /// <param name="parameters">Vehicle parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all vehicles</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetVehiclesRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetVehiclesAsync(
        [FromQuery] GetVehiclesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
        && !_propertyMappingService.ValidMappingExistsFor<VehicleUiModel, Vehicle>(parameters.OrderBy))
        {
            return BadRequest("VEHICLES_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<VehicleUiModel>
            (parameters.Fields))
        {
            return BadRequest("VEHICLES_FIELDS_ERROR");
        }

        var response = await _mediator.Send(new GetVehiclesQuery(parameters));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:GetVehiclesRoot -- Message:VEHICLES_FETCH" +
                  $" -- Datetime:{DateTime.UtcNow} -- Action: GetVehiclesAsync");
            return NotFound("VEHICLES_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetVehicleByIdAsync", "GetVehiclesAsync");
        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one vehicle
    /// </summary>
    /// <param name="id">Vehicle Id for fetching</param>
    /// <remarks>Fetch one vehicle </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetVehicleByIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetVehicleByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new GetVehicleByIdQuery(id));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:GetVehicleByIdRoot -- Message:VEHICLE_FETCH" +
                  $" -- Datetime:{DateTime.UtcNow} -- Action: GetVehicleByIdAsync");
            return NotFound("VEHICLE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Post - Create a vehicle
    /// </summary>
    /// <param name="request">CreateVehicleResourceParameters for creation</param>
    /// <remarks>Create vehicle </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostVehicleRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostVehicleAsync([FromBody] CreateVehicleResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(
            new CreateVehicleCommand(userAudit.Model.Id, request)
        );

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:PostVehicleRoot -- Message:VEHICLE_CREATION" +
                  $" -- Datetime:{DateTime.UtcNow} -- Action: PostVehicleAsync");
            return NotFound("VEHICLE_CREATED_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Put - Update a vehicle
    /// </summary>
    /// <param name="id">Vehicle Id for modification</param>
    /// <param name="request">UpdateVehicleResourceParameters for modification</param>
    /// <remarks>Update vehicle </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateVehicleRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateVehicleAsync(long id, [FromBody] UpdateVehicleResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new UpdateVehicleCommand(id, userAudit.Model.Id, request));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:UpdateVehicleRoot -- Message:VEHICLE_UPDATE" +
                  $" -- Datetime:{DateTime.UtcNow} -- Action: UpdateVehicleAsync");
            return NotFound("VEHICLE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing vehicle - Soft Delete
    /// </summary>
    /// <param name="id">Vehicle Id for deletion</param>
    /// <param name="request">DeleteVehicleResourceParameters for deletion</param>
    /// <remarks>Delete existing vehicle </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftVehicleRoot")]
    [ValidateModel]
    public async Task<IActionResult> DeleteSoftVehicleAsync(long id, [FromBody] DeleteVehicleResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new DeleteSoftVehicleCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:DeleteSoftVehicleRoot -- Message:VEHICLE_SOFT_DELETION" +
                  $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteSoftVehicleAsync");
            return NotFound("VEHICLE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing vehicle - Hard Delete
    /// </summary>
    /// <param name="id">Vehicle Id for deletion</param>
    /// <remarks>Delete existing vehicle </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardVehicleRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardVehicleAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!this.IsSuFromClaims())
        {
            Log.Error(
              $"--Method:DeleteHardVehicleRoot -- Message:VEHICLES_HARD_DELETION" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardVehicleAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }
        var response = await _mediator.Send(new DeleteHardVehicleCommand(id));

        if (response.IsNull())
        {
            Log.Error(
                  $"--Method:DeleteHardVehicleRoot -- Message:VEHICLES_HARD_DELETION_AFTER_DELETION_ERROR" +
                  $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardVehicleAsync");
            return NotFound("VEHICLE_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }
}// Class: VehiclesController
