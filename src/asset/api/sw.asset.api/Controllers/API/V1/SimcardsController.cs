using System;
using System.Threading.Tasks;
using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.ResourceParameters.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.model.Devices.Simcards;
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

namespace sw.asset.api.Controllers.API.V1;

/// <summary>
/// Class : Simcard Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(Roles = "SU, ADMIN")]

public class SimcardsController : BaseController {
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
    public SimcardsController(
        IMediator mediator,
        IUrlHelper urlHelper,
        ITypeHelperService typeHelperService,
        IPropertyMappingService propertyMappingService) {
        this._mediator = mediator;
        this._urlHelper = urlHelper;
        this._typeHelperService = typeHelperService;
        this._propertyMappingService = propertyMappingService;
    }

    /// <summary>
    /// Get - Fetch all Simcards
    /// </summary>
    /// <param name="parameters">Simcard parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all Simcards </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetSimcardsAsync")]
    [ValidateModel]
    public async Task<IActionResult> GetSimcardsAsync(
        [FromQuery] GetSimcardsResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType) {

        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull()) {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<SimcardUiModel, Simcard>(parameters.OrderBy)) {
            return this.BadRequest("SIMCARDS_MODEL_ERROR");
        }

        if (!this._typeHelperService.TypeHasProperties<SimcardUiModel>
                (parameters.Fields)) {
            return this.BadRequest("SIMCARDS_FIELDS_ERROR");
        }

        BusinessResult<PagedList<SimcardUiModel>> fetchedSimcards;
        if (this.IsSuFromClaims()) {
            try {
                Log.Information(
                    $"--Method:GetSimcardsAsync -- Message:SIMCARDS_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetSimcardsAsync");
                fetchedSimcards = await this._mediator.Send(new GetSimcardsQuery(parameters));
                Log.Information(
                    $"--Method:GetSimcardsAsync -- Message:SIMCARDS_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just After : GetSimcardsAsync");
            } catch (Exception e) {
                Log.Error(
                    $"--Method:GetSimcardsAsync -- Message:SIMCARDS_FETCH_ERROR" +
                    $" -- Datetime:{DateTime.UtcNow} -- SimcardInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                return this.BadRequest("ERROR_FETCH_SIMCARDS");
            }
        } else {
            try {
                Log.Information(
                    $"--Method:GetSimcardsAsync -- Message:SIMCARDS_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetSimcardsAsync");
                //Fetch With Company ?
                fetchedSimcards = await this._mediator.Send(new GetSimcardsQuery(parameters));
                Log.Information(
                    $"--Method:GetSimcardsAsync -- Message:SIMCARDS_FETCH" +
                    $" -- Datetime:{DateTime.UtcNow} -- Just After : GetSimcardsAsync");
            } catch (Exception e) {
                Log.Error(
                    $"--Method:GetSimcardsAsync -- Message:SIMCARDS_FETCH_ERROR" +
                    $" -- Datetime:{DateTime.UtcNow} -- SimcardInfo:{e.Message} - Details :  {e.InnerException?.Message}");
                return this.BadRequest("ERROR_FETCH_SIMCARDS");
            }
        }

        if (fetchedSimcards.IsNull()) {
            return this.NotFound("SIMCARDS_NOT_FOUND");
        }

        if (!fetchedSimcards.IsSuccess()) {
            return this.OkOrBadRequest(fetchedSimcards.BrokenRules);
        }

        if (fetchedSimcards.Status == BusinessResultStatus.Fail) {
            return this.OkOrNoResult(fetchedSimcards.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedSimcards.Model, mediaType,
            parameters, this._urlHelper, "GetSimcardByIdAsync", "GetSimcardsAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one Simcard
    /// </summary>
    /// <param name="id">Simcard Id for fetching</param>
    /// <remarks>Fetch one Simcard </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetSimcardByIdAsync")]
    [ValidateModel]
    public async Task<IActionResult> GetSimcardByIdAsync(long id) {

        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull()) {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var fetchedSimcard = await this._mediator.Send(new GetSimcardByIdQuery(id));

        if (fetchedSimcard.IsNull()) {
            return this.NotFound("SIMCARD_NOT_FOUND");
        }

        if (!fetchedSimcard.IsSuccess()) {
            return this.OkOrBadRequest(fetchedSimcard.BrokenRules);
        }

        if (fetchedSimcard.Status == BusinessResultStatus.Fail) {
            return this.OkOrNoResult(fetchedSimcard.BrokenRules);
        }

        return this.Ok(fetchedSimcard);
    }

    /// <summary>
    /// Post - Create a Simcard
    /// </summary>
    /// <param name="request">CreateSimcardResourceParameters for creation</param>
    /// <remarks>Create Simcard </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostSimcardAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> PostSimcardAsync([FromBody] CreateSimcardResourceParameters request) {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull()) {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var createdSimcard = await this._mediator.Send(
            new CreateSimcardCommand(userAudit.Model.Id, request)
        );

        if (createdSimcard.IsNull()) {
            return this.NotFound("ERROR_SIMCARD_CREATED_NOT_FOUND");
        }

        if (!createdSimcard.IsSuccess()) {
            return this.OkOrBadRequest(createdSimcard.BrokenRules);
        }

        if (createdSimcard.Status == BusinessResultStatus.Fail) {
            return this.OkOrNoResult(createdSimcard.BrokenRules);
        }

        return this.CreatedOrNoResult(createdSimcard);
    }

    /// <summary>
    /// Put - Update a Simcard
    /// </summary>
    /// <param name="id">Simcard Id for modification</param>
    /// <param name="request">UpdateSimcardResourceParameters for modification</param>
    /// <remarks>Update Simcard </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateSimcardAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> UpdateSimcardAsync(long id, [FromBody] UpdateSimcardResourceParameters request) {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull()) {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await this._mediator.Send(new UpdateSimcardCommand(id, userAudit.Model.Id, request));

        if (response == null) {
            return this.NotFound("SIMCARD_NOT_FOUND");
        }

        if (!response.IsSuccess()) {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail) {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing Simcard - Soft Delete
    /// </summary>
    /// <param name="id">Simcard Id for deletion</param>
    /// <param name="request">DeleteSimcardResourceParameters for deletion</param>
    /// <remarks>Delete existing Simcard </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftSimcardAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> DeleteSoftSimcardAsync(long id, [FromBody] DeleteSimcardResourceParameters request) {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model == null)
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        var response = await this._mediator.Send(new DeleteSoftSimcardCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull()) {
            return this.NotFound("SIMCARD_NOT_FOUND");
        }

        if (!response.IsSuccess()) {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail) {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing Simcard - Hard Delete
    /// </summary>
    /// <param name="id">Simcard Id for deletion</param>
    /// <remarks>Delete existing Simcard </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardSimcardRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardSimcardAsync(long id) {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        if (!this.IsSuFromClaims())
        {
            Log.Error(
                $"--Method:DeleteHardSimcardRoot -- Message:SIMCARDS_HARD_DELETION" +
                $" -- Datetime:{DateTime.Now} -- Action: DeleteHardSimcardAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }

        var response = await this._mediator.Send(new DeleteHardSimcardCommand(id));

        if (response.IsNull()) {
            return this.NotFound("SIMCARD_NOT_FOUND");
        }

        if (!response.IsSuccess()) {
            return this.OkOrBadRequest(response.BrokenRules);
        }

        if (response.Status == BusinessResultStatus.Fail) {
            return this.OkOrNoResult(response.BrokenRules);
        }

        return this.Ok(response);
    }
}