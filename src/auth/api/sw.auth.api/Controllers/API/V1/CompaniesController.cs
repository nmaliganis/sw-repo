using sw.auth.api.Validators;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.ResourceParameters.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.auth.model.Companies;
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

namespace sw.auth.api.Controllers.API.V1;

/// <summary>
/// Company : CompaniesController
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
        this._mediator = mediator;
        this._urlHelper = urlHelper;
        this._typeHelperService = typeHelperService;
        this._propertyMappingService = propertyMappingService;
    }

    /// <summary>
    /// Get - Fetch all Companies
    /// </summary>
    /// <param name="parameters">Company parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all Companies </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "FetchCompaniesRoot")]
    [ValidateModel]
    public async Task<IActionResult> FetchCompaniesAsync([FromQuery] GetCompaniesResourceParameters parameters, [FromHeader(Name = "Accept")] string mediaType)
    {

        var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model.IsNull())
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        if (!this._propertyMappingService.ValidMappingExistsFor<CompanyUiModel, Company>
                (parameters.OrderBy))
        {
            return this.BadRequest("ERROR_RESOURCE_PARAMETER");
        }

        if (!this._typeHelperService.TypeHasProperties<CompanyUiModel>
                (parameters.Fields))
        {
            return this.BadRequest("ERROR_FIELDS_PARAMETER");
        }

        var companyIdFromClaims = this.GetCompanyFromClaims();

        if (companyIdFromClaims <= 0)
        {
            return this.BadRequest("ERROR_INVALID_COMPANY");
        }

        try
        {
            PagedList<CompanyUiModel> companiesQueryable;
            if (this.IsSuFromClaims())
            {
                Log.Information(
                    $"--Method:GetCompaniesRoot -- Message:COMPANY_FETCH" +
                    $" -- Datetime:{DateTime.Now} -- Just Before : GetCompaniesAsync");
                var fetchedCompanies =
                    await this._mediator.Send(new GetCompaniesQuery(parameters));
                if (fetchedCompanies.IsNull())
                {
                    return this.NotFound("ERROR_FETCH_COMPANIES");
                }
                if (!fetchedCompanies.IsSuccess())
                {
                    return this.OkOrBadRequest(fetchedCompanies.BrokenRules);
                }

                if (fetchedCompanies.Status == BusinessResultStatus.Fail)
                {
                    return this.OkOrNoResult(fetchedCompanies.BrokenRules);
                }

                companiesQueryable = fetchedCompanies.Model;

                Log.Information(
                    $"--Method:GetCompaniesRoot -- Message:COMPANY_FETCH" +
                    $" -- Datetime:{DateTime.Now} -- Just After : GetCompaniesAsync");
                var responseWithMetaData = this.CreateOkWithMetaData(companiesQueryable, mediaType,
                    parameters, this._urlHelper, "GetCompanyByIdAsync", "GetCompaniesAsync");
                return this.Ok(responseWithMetaData);
            }
            else
            {
                Log.Information(
                    $"--Method:GetCompaniesRoot -- Message:COMPANY_FETCH_BY_USER" +
                    $" -- Datetime:{DateTime.Now} -- Just Before : GetCompaniesByIdAsync");
                var fetchedCompanies =
                    await this._mediator.Send(new GetCompaniesByUserQuery(companyIdFromClaims, parameters));
                if (fetchedCompanies.IsNull())
                {
                    return this.NotFound("ERROR_FETCH_COMPANY_BY_USER");
                }

                if (!fetchedCompanies.IsSuccess())
                {
                    return this.OkOrBadRequest(fetchedCompanies.BrokenRules);
                }

                if (fetchedCompanies.Status == BusinessResultStatus.Fail)
                {
                    return this.OkOrNoResult(fetchedCompanies.BrokenRules);
                }

                companiesQueryable = fetchedCompanies.Model;

                Log.Information(
                    $"--Method:GetCompaniesRoot -- Message:COMPANY_FETCH" +
                    $" -- Datetime:{DateTime.Now} -- Just After : GetCompaniesByInstitutionAsync");

                var responseWithMetaData = this.CreateOkWithMetaData(companiesQueryable, mediaType,
                    parameters, this._urlHelper, "GetCompaniesAsync", "GetCompaniesAsync");
                return this.Ok(responseWithMetaData);
            }
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:GetCompaniesRoot -- Message:COMPANY_FETCH_ERROR" +
                $" -- Datetime:{DateTime.Now} -- CompanyInfo:{e.Message} - Details :  {e.InnerException?.Message}");
            return this.BadRequest("ERROR_FETCH_COMPANIES");
        }
    }

    /// <summary>
    /// Get - Fetch one Company
    /// </summary>
    /// <param name="id">Company Id for fetching</param>
    /// <remarks>Fetch one Company </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetCompanyByIdAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> GetCompanyByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model == null)
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        var companyIdFromClaims = this.GetCompanyFromClaims();

        if (companyIdFromClaims <= 0)
        {
            return this.BadRequest("ERROR_INVALID_COMPANY");
        }

        var fetchedCompany = await this._mediator.Send(new GetCompanyByIdQuery(id));

        if (fetchedCompany.IsNull())
        {
            return this.NotFound();
        }

        if (!fetchedCompany.IsSuccess())
        {
            return this.OkOrBadRequest(fetchedCompany.BrokenRules);
        }

        if (fetchedCompany.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(fetchedCompany.BrokenRules);
        }

        return this.Ok(fetchedCompany);
    }

    /// <summary>
    /// Post - Create a Company
    /// </summary>
    /// <param name="companyForCreation">CreateCompanyResourceParameters for creation</param>
    /// <remarks>Create Company </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "CreateCompanyRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyResourceParameters companyForCreation)
    {
        var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit == null)
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        if (!this.IsSuFromClaims())
        {
            return this.BadRequest("ERROR_UNAUTHORIZED_FOR_CREATION_ACTION");
        }

        var createdCompany = await this._mediator.Send(new CreateCompanyCommand(userAudit.Model.Id, companyForCreation.Name, companyForCreation.CodeErp, companyForCreation.Description));

        if (createdCompany.IsNull())
        {
            return this.NotFound("ERROR_COMPANY_CREATED_NOT_FOUND");
        }

        if (!createdCompany.IsSuccess())
        {
            return this.OkOrBadRequest(createdCompany.BrokenRules);
        }

        if (createdCompany.Status == BusinessResultStatus.Fail)
        {
            Log.Information(
                $"--Method:PostCompanyRouteAsync -- Message:Company_CREATION_SUCCESSFULLY" +
                $" -- Datetime:{DateTime.Now} -- CompanyInfo:{createdCompany.Model.Name}");
            return this.OkOrNoResult(createdCompany.BrokenRules);
        }


        return this.CreatedOrNoResult(createdCompany);
    }

    /// <summary>
    /// Put - Update a Company
    /// </summary>
    /// <param name="id">Company Id for modification</param>
    /// <param name="request">UpdateCompanyResourceParameters for modification</param>
    /// <remarks>Update Company </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateCompanyAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> UpdateCompanyAsync(long id, [FromBody] UpdateCompanyResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await this._mediator.Send(new UpdateCompanyCommand(userAudit.Model.Id, id, request.Name, request.CodeErp, request.Description));

        if (response == null)
        {
            return this.NotFound("COMPANY_NOT_FOUNT");
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
    /// Delete - Delete an existing Company - Soft Delete
    /// </summary>
    /// <param name="id">Company Id for deletion</param>
    /// <param name="request">DeleteCompanyResourceParameters for deletion</param>
    /// <remarks>Delete existing Company </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftCompanyAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU, ADMIN")]
    public async Task<IActionResult> DeleteSoftCompanyAsync(long id, [FromBody] DeleteCompanyResourceParameters request)
    {

        var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model == null)
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        var deletedCompany = await this._mediator.Send(new DeleteSoftCompanyCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (deletedCompany.IsNull())
        {
            return this.NotFound();
        }

        if (!deletedCompany.IsSuccess())
        {
            return this.OkOrBadRequest(deletedCompany.BrokenRules);
        }

        if (deletedCompany.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(deletedCompany.BrokenRules);
        }

        return this.Ok(deletedCompany);
    }

    /// <summary>
    /// Delete - Delete an existing Company - Hard Delete
    /// </summary>
    /// <param name="id">Company Id for deletion</param>
    /// <remarks>Delete existing Company </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardCompanyAsync")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardCompanyAsync(long id)
    {

        var userAudit = await this._mediator.Send(new GetUserByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.Model == null)
        {
            return this.BadRequest("ERROR_VALIDATION_MODEL_AUDIT");
        }

        if (!this.IsSuFromClaims())
        {
            Log.Error(
                $"--Method:GetCompaniesRoot -- Message:COMPANY_HARD_DELETION" +
                $" -- Datetime:{DateTime.Now} -- Action: GetCompaniesAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }

        var hardDeletedCompany = await this._mediator.Send(new DeleteHardCompanyCommand(id));

        if (hardDeletedCompany.IsNull())
        {
            return this.NotFound("ERROR_HARD_DELETION_NOT_EXECUTED");
        }

        if (!hardDeletedCompany.IsSuccess())
        {
            return this.OkOrBadRequest(hardDeletedCompany.BrokenRules);
        }

        if (hardDeletedCompany.Status == BusinessResultStatus.Fail)
        {
            return this.OkOrNoResult(hardDeletedCompany.BrokenRules);
        }

        return this.Ok(hardDeletedCompany);
    }
}//Class : CompaniesController
