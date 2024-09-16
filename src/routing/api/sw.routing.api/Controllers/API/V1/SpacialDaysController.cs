using sw.routing.api.Validators;
using sw.routing.common.dtos.ResourceParameters.Itineraries;
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
using System.Net;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.SpecialDays;
using sw.routing.common.dtos.ResourceParameters.SpecialDays;
using sw.routing.common.dtos.Vms.SpecialDays;
using sw.routing.model.SpecialDays;

namespace sw.routing.api.Controllers.API.V1;

/// <summary>
/// SpecialDay Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SpecialDaysController : BaseController
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
    public SpecialDaysController(
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
    /// Get - Fetch all SpecialDays
    /// </summary>
    /// <param name="parameters">SpecialDay parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all SpecialDays </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "FetchSpecialDaysRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(BusinessResult<PagedList<SpecialDayUiModel>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FetchSpecialDaysAsync(
        [FromQuery] GetItinerariesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        //var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        //if (userAudit.Model == null)
        //{
        //    return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        //}

        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<SpecialDayUiModel, SpecialDay>(parameters.OrderBy))
        {
            return this.BadRequest("ERROR_RESOURCE_PARAMETER");
        }

        if (!this._typeHelperService.TypeHasProperties<SpecialDayUiModel>
                (parameters.Fields))
        {
            return this.BadRequest("ERROR_FIELDS_PARAMETER");
        }

        var fetchedSpecialDays = await this._mediator.Send(new GetSpecialDaysQuery(parameters));

        if (fetchedSpecialDays.IsNull())
        {
            return this.NotFound("ERROR_FETCH_SPECIAL_DAYS");
        }

        if (!fetchedSpecialDays.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedSpecialDays.BrokenRules);
        }

        if (fetchedSpecialDays.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedSpecialDays.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedSpecialDays.Model, mediaType,
            parameters, this._urlHelper, "GetSpecialDayByIdAsync", "FetchSpecialDaysAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one SpecialDay
    /// </summary>
    /// <param name="id">SpecialDay Id for fetching</param>
    /// <remarks>Fetch one SpecialDay </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetSpecialDayByIdAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(SpecialDayUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetSpecialDayByIdAsync(long id)
    {
        //var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        //if (userAudit.Model == null)
        //{
        //    return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        //}

        var fetchedSpecialDay = await this._mediator.Send(new GetSpecialDayByIdQuery(id));

        if (fetchedSpecialDay.IsNull())
        {
            return this.NotFound("ERROR_FETCH_SPECIAL_DAY");
        }

        if (!fetchedSpecialDay.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedSpecialDay.BrokenRules);
        }

        if (fetchedSpecialDay.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedSpecialDay.BrokenRules);
        }

        return this.Ok(fetchedSpecialDay);
    }

    /// <summary>
    /// Post - Create a SpecialDay
    /// </summary>
    /// <param name="specialDayForCreation">CreateSpecialDayResourceParameters for creation</param>
    /// <remarks>Create SpecialDay </remarks>
    /// <response code="201">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostSpecialDayAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(SpecialDayUiModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PostSpecialDayAsync([FromBody] CreateSpecialDayResourceParameters specialDayForCreation)
    {
        //var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        //if (userAudit == null)
        //{
        //    return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        //}

        if (!this.IsSuFromClaims())
        {
            return this.BadRequest("ERROR_UNAUTHORIZED_FOR_CREATION_ACTION");
        }

        //var createdSpecialDay = await this._mediator.Send(new CreateSpecialDayCommand(userAudit.Model.Id, SpecialDayForCreation.Name));
        var createdSpecialDay = await this._mediator.Send(new CreateSpecialDayCommand(0, specialDayForCreation.Name));

        if (createdSpecialDay.IsNull())
        {
            return this.NotFound("ERROR_SPECIAL_DAY_CREATED_NOT_FOUND");
        }

        if (!createdSpecialDay.IsSuccess())
        {
            return this.OkOrBadRequest(createdSpecialDay.BrokenRules);
        }

        if (createdSpecialDay.Status == BusinessResultStatus.Fail)
        {
            Log.Information(
                $"--Method:PostSpecialDayRouteAsync -- Message:SPECIAL_DAY_CREATION_SUCCESSFULLY" +
                $" -- Datetime:{DateTime.Now} -- SpecialDayInfo:{createdSpecialDay.Model.Name}");
            return this.OkOrNoResult(createdSpecialDay.BrokenRules);
        }
        return this.CreatedOrNoResult(createdSpecialDay);
    }

    /// <summary>
    /// Put - Update a SpecialDay
    /// </summary>
    /// <param name="id">SpecialDay Id for modification</param>
    /// <param name="request">UpdateSpecialDayResourceParameters for modification</param>
    /// <remarks>Update SpecialDay </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateSpecialDayAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(SpecialDayUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateSpecialDayAsync(long id, [FromBody] UpdateSpecialDayResourceParameters request)
    {
        //var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        //if (userAudit.IsNull())
        //{
        //    return this.NotFound("USER_AUDIT_NOT_FOUND");
        //}

        //var response = await this._mediator.Send(new UpdateSpecialDayCommand(userAudit.Model.Id, id, request.Name));
        var response = await this._mediator.Send(new UpdateSpecialDayCommand(0, id, request.Name));

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
    /// Delete - Delete an existing SpecialDay - Soft Delete
    /// </summary>
    /// <param name="id">SpecialDay Id for deletion</param>
    /// <param name="request">DeleteSpecialDayResourceParameters for deletion</param>
    /// <remarks>Delete existing SpecialDay </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftSpecialDayAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    [ProducesResponseType(typeof(SpecialDayUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteSoftSpecialDayAsync(long id, [FromBody] DeleteSpecialDayResourceParameters request)
    {
        //var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        //if (userAudit.Model == null)
        //{
        //    return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        //}

        //var deletedSpecialDay = await this._mediator.Send(new DeleteSoftSpecialDayCommand(id, userAudit.Model.Id, request.DeletedReason));
        var deletedSpecialDay = await this._mediator.Send(new DeleteSoftSpecialDayCommand(id, 0, request.DeletedReason));

        if (deletedSpecialDay.IsNull())
        {
            return this.NotFound("ERROR_DELETE_SPECIAL_DAY");
        }

        if (!deletedSpecialDay.IsSuccess())
        {
            return this.OkOrBadRequest(deletedSpecialDay.BrokenRules);
        }

        if (deletedSpecialDay.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(deletedSpecialDay.BrokenRules);
        }

        return this.Ok(deletedSpecialDay);
    }

    /// <summary>
    /// Delete - Delete an existing SpecialDay - Hard Delete
    /// </summary>
    /// <param name="id">SpecialDay Id for deletion</param>
    /// <remarks>Delete existing SpecialDay </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardSpecialDayAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    [ProducesResponseType(typeof(SpecialDayUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteHardSpecialDayAsync(long id)
    {

        //var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        //if (userAudit.Model == null)
        //{
        //    return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        //}

        //var response = await this._mediator.Send(new DeleteHardSpecialDayCommand(id));
        var response = await this._mediator.Send(new DeleteHardSpecialDayCommand(id));

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
}//Class : SpecialDaysController