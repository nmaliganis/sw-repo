using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.model.Companies;
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
/// Company Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class CompaniesController : BaseController
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
    public CompaniesController(
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
    /// Get - Fetch all companies
    /// </summary>
    /// <param name="parameters">Company parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all companies </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetCompaniesRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetCompaniesAsync(
        [FromQuery] GetCompaniesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
        && !_propertyMappingService.ValidMappingExistsFor<CompanyUiModel, Company>(parameters.OrderBy))
        {
            return BadRequest("COMPANY_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<CompanyUiModel>
            (parameters.Fields))
        {
            return BadRequest("COMPANY_FIELDS_ERROR");
        }

        var response = await _mediator.Send(new GetCompaniesQuery(parameters));

        if (response.IsNull())
        {
            return NotFound("COMPANIES_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters, _urlHelper, "GetCompanyByIdAsync", "GetCompaniesAsync");
        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one company
    /// </summary>
    /// <param name="id">Company Id for fetching</param>
    /// <remarks>Fetch one company </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetCompanyByIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetCompanyByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new GetCompanyByIdQuery(id));

        if (response.IsNull())
        {
            return NotFound("COMPANY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }


    /// <summary>
    /// Get - Fetch Zones By Company
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch one company </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("zones", Name = "GetZonesByCompanyIdRoot")]
    [ValidateModel]
    public async Task<IActionResult> GetZonesByCompanyIdAsync([FromQuery] GetZonesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<CompanyUiModel, Company>(parameters.OrderBy))
        {
            return BadRequest("ZONE_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<CompanyUiModel>
                (parameters.Fields))
        {
            return BadRequest("ZONE_FIELDS_ERROR");
        }

        var companyIdFromClaims = this.GetCompanyFromClaims();

        if (companyIdFromClaims <= 0)
        {
            return BadRequest("ZONE_COMPANY_ERROR");
        }

        var response = await _mediator.Send(new GetZonesByCompanyIdQuery(companyIdFromClaims, parameters));

        if (response.IsNull())
        {
            return NotFound("ZONES_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(response.Model, mediaType, parameters,
            _urlHelper, "GetZonesByCompanyIdAsync", "GetZonesByCompanyAsync");
        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Post - Create a company
    /// </summary>
    /// <param name="request">CreateCompanyResourceParameters for creation</param>
    /// <remarks>Create company </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostCompanyRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostCompanyAsync([FromBody] CreateCompanyResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(
            new CreateCompanyCommand(userAudit.Model.Id, request.Name, request.CodeErp, request.Description)
        );

        if (response.IsNull())
        {
            return NotFound("COMPANY_CREATED_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }


    /// <summary>
    /// Put - Update a company
    /// </summary>
    /// <param name="companyId">Company Id for modification</param>
    /// <param name="request">UpdateCompanyResourceParameters for modification</param>
    /// <remarks>Update company </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("zones", Name = "UpdateCompanyWithNewZoneRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateCompanyWithNewZoneAsync(long companyId, [FromBody] UpdateCompanyWithZoneResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new UpdateCompanyWithZoneCommand(userAudit.Model.Id, companyId, request.Zones));

        if (response.IsNull())
        {
            return NotFound("COMPANY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }


    /// <summary>
    /// Put - Update a company
    /// </summary>
    /// <param name="id">Company Id for modification</param>
    /// <param name="request">UpdateCompanyResourceParameters for modification</param>
    /// <remarks>Update company </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateCompanyRoot")]
    [ValidateModel]
    public async Task<IActionResult> UpdateCompanyAsync(long id, [FromBody] UpdateCompanyResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new UpdateCompanyCommand(userAudit.Model.Id, id, request.Name, request.CodeErp, request.Description));

        if (response.IsNull())
        {
            return NotFound("COMPANY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing company - Soft Delete
    /// </summary>
    /// <param name="id">Company Id for deletion</param>
    /// <param name="request">DeleteCompanyResourceParameters for deletion</param>
    /// <remarks>Delete existing company </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftCompanyAsync")]
    [ValidateModel]
    public async Task<IActionResult> DeleteSoftCompanyAsync(long id, [FromBody] DeleteCompanyResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new DeleteSoftCompanyCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            return NotFound("COMPANY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing company - Hard Delete
    /// </summary>
    /// <param name="id">Company Id for deletion</param>
    /// <remarks>Delete existing company </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardCompanyRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardCompanyAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!this.IsSuFromClaims())
        {
            Log.Error(
              $"--Method:DeleteHardCompanyRoot -- Message:COMPANIES_HARD_DELETION_NOT_SU" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardCompanyAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }
        var response = await _mediator.Send(new DeleteHardCompanyCommand(id));

        if (response.IsNull())
        {
            return NotFound("COMPANY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }
}//Class: CompaniesController
