using sw.routing.api.Validators;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.ResourceParameters.Templates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
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
using sw.routing.model.ItineraryTemplates;

namespace sw.routing.api.Controllers.API.V1;

/// <summary>
/// ItineraryTemplate Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class ItineraryTemplatesController : BaseController
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
    public ItineraryTemplatesController(
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
    /// Get - Fetch all ItineraryTemplates
    /// </summary>
    /// <param name="parameters">ItineraryTemplate parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all ItineraryTemplates </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "FetchItineraryTemplatesRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(BusinessResult<PagedList<ItineraryTemplateUiModel>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FetchItineraryTemplatesAsync(
        [FromQuery] GetItinerariesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !this._propertyMappingService.ValidMappingExistsFor<ItineraryTemplateUiModel, ItineraryTemplate>(parameters.OrderBy))
        {
            return this.BadRequest("ERROR_RESOURCE_PARAMETER");
        }

        if (!this._typeHelperService.TypeHasProperties<ItineraryTemplateUiModel>
                (parameters.Fields))
        {
            return this.BadRequest("ERROR_FIELDS_PARAMETER");
        }

        BusinessResult<PagedList<ItineraryTemplateUiModel>> fetchedItineraryTemplates;
        try
        {
            Log.Information(
                $"--Method:FetchItineraryTemplatesAsync -- Message:ITINERARY_TEMPLATE_FETCH" +
                $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetItineraryTemplatesQuery");
            fetchedItineraryTemplates = await this._mediator.Send(new GetItineraryTemplatesQuery(parameters));
            Log.Information(
                $"--Method:FetchItineraryTemplatesAsync -- Message:ITINERARY_TEMPLATE_FETCH" +
                $" -- Datetime:{DateTime.UtcNow} -- Just After : GetItineraryTemplatesQuery");
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:FetchItineraryTemplatesAsync -- Message:ITINERARY_TEMPLATE_FETCH_ERROR" +
                $" -- Datetime:{DateTime.UtcNow} -- ItineraryTemplateInfo:{e.Message} - Details :  {e.InnerException?.Message}");
            return this.BadRequest("ERROR_FETCH_ITINERARY_TEMPLATE");
        }

        if (fetchedItineraryTemplates.IsNull())
        {
            return this.NotFound("ERROR_FETCH_ITINERARY_TEMPLATES");
        }

        if (!fetchedItineraryTemplates.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedItineraryTemplates.BrokenRules);
        }

        if (fetchedItineraryTemplates.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedItineraryTemplates.BrokenRules);
        }

        var responseWithMetaData = this.CreateOkWithMetaData(fetchedItineraryTemplates.Model, mediaType,
            parameters, this._urlHelper, "GetItineraryTemplateByIdAsync", "FetchItineraryTemplatesAsync");
        return this.Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one ItineraryTemplate
    /// </summary>
    /// <param name="id">ItineraryTemplate Id for fetching</param>
    /// <remarks>Fetch one ItineraryTemplate </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetItineraryTemplateByIdAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryTemplateUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetItineraryTemplateByIdAsync(long id)
    {
        var fetchedItineraryTemplate = await this._mediator.Send(new GetItineraryTemplateByIdQuery(id));

        if (fetchedItineraryTemplate.IsNull())
        {
            return this.NotFound("ERROR_FETCH_ITINERARY_TEMPLATE");
        }

        if (!fetchedItineraryTemplate.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedItineraryTemplate.BrokenRules);
        }

        if (fetchedItineraryTemplate.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedItineraryTemplate.BrokenRules);
        }

        return this.Ok(fetchedItineraryTemplate);
    }

    /// <summary>
    /// Post - Create a ItineraryTemplate
    /// </summary>
    /// <param name="itineraryTemplateForCreation">CreateItineraryTemplateResourceParameters for creation</param>
    /// <remarks>Create ItineraryTemplate </remarks>
    /// <response code="201">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "CreateItineraryTemplateRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryTemplateUiModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateItineraryTemplateAsync([FromBody] CreateItineraryTemplateResourceParameters itineraryTemplateForCreation)
    {
        var userAudit = this.GetMemberFromClaims();

        if (userAudit <= 0)
        {
            Log.Error(
                $"--Method:CreateItineraryTemplateRoot -- Message:CreateItineraryTemplateAsync" +
                $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var createdItineraryTemplate = 
            await this._mediator.Send(new CreateItineraryTemplateCommand(userAudit, itineraryTemplateForCreation));

        if (createdItineraryTemplate.IsNull())
        {
            return this.NotFound("ERROR_ITINERARY_TEMPLATE_CREATED_NOT_FOUND");
        }

        if (!createdItineraryTemplate.IsSuccess())
        {
            return this.OkOrBadRequest(createdItineraryTemplate.BrokenRules);
        }

        if (createdItineraryTemplate.Status == BusinessResultStatus.Fail)
        {
            Log.Information(
                $"--Method:PostItineraryTemplateRouteAsync -- Message:ITINERARY_TEMPLATE_CREATION_SUCCESSFULLY" +
                $" -- Datetime:{DateTime.Now} -- ItineraryTemplateInfo:{createdItineraryTemplate.Model.Name}");
            return this.OkOrNoResult(createdItineraryTemplate.BrokenRules);
        }
        return this.CreatedOrNoResult(createdItineraryTemplate);
    }

    /// <summary>
    /// Put - Update a ItineraryTemplate
    /// </summary>
    /// <param name="id">ItineraryTemplate Id for modification</param>
    /// <param name="request">UpdateItineraryTemplateResourceParameters for modification</param>
    /// <remarks>Update ItineraryTemplate </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateItineraryTemplateRoot")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryTemplateUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateItineraryTemplateAsync(long id, [FromBody] UpdateItineraryTemplateResourceParameters request)
    {
        var userAudit = this.GetMemberFromClaims();

        if (userAudit <= 0)
        {
            Log.Error(
                $"--Method:UpdateItineraryTemplateRoot -- Message:UpdateItineraryTemplateAsync" +
                $" -- Datetime:{DateTime.Now} -- Action: userAudit.IsNull()");
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = 
            await this._mediator.Send(new UpdateItineraryTemplateCommand(userAudit, id, request));

        if (response == null)
        {
            return this.NotFound("ITINERARY_TEMPLATE_NOT_FOUNT");
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
    /// Delete - Delete an existing ItineraryTemplate - Soft Delete
    /// </summary>
    /// <param name="id">ItineraryTemplate Id for deletion</param>
    /// <param name="request">DeleteItineraryTemplateResourceParameters for deletion</param>
    /// <remarks>Delete existing ItineraryTemplate </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftItineraryTemplateAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryTemplateUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteSoftItineraryTemplateAsync(long id, [FromBody] DeleteItineraryTemplateResourceParameters request)
    {
        var deletedItineraryTemplate = await this._mediator.Send(new DeleteSoftItineraryTemplateCommand(id, 0, request.DeletedReason));

        if (deletedItineraryTemplate.IsNull())
        {
            return this.NotFound("ERROR_DELETE_ITINERARY_TEMPLATE");
        }

        if (!deletedItineraryTemplate.IsSuccess())
        {
            return this.OkOrBadRequest(deletedItineraryTemplate.BrokenRules);
        }

        if (deletedItineraryTemplate.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(deletedItineraryTemplate.BrokenRules);
        }

        return this.Ok(deletedItineraryTemplate);
    }

    /// <summary>
    /// Delete - Delete an existing ItineraryTemplate - Hard Delete
    /// </summary>
    /// <param name="id">ItineraryTemplate Id for deletion</param>
    /// <remarks>Delete existing ItineraryTemplate </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardItineraryTemplateAsync")]
    [ValidateModel]
    [ProducesResponseType(typeof(ItineraryTemplateUiModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteHardItineraryTemplateAsync(long id)
    {
        var response = await this._mediator.Send(new DeleteHardItineraryTemplateCommand(id));

        if (response.IsNull())
        {
            return this.NotFound("ERROR_DELETE_ITINERARY_TEMPLATE");
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

}//Class : ItineraryTemplatesController