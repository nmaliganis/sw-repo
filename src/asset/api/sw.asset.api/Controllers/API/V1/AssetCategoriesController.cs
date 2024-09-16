using sw.asset.api.Validators;
using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.ResourceParameters.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.model.Assets.Categories;
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
/// AssetCategory Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(Roles = "SU, ADMIN")]
public class AssetCategoriesController : BaseController
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
    public AssetCategoriesController(
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
    /// Get - Fetch all asset categories
    /// </summary>
    /// <param name="parameters">AssetCategory parameters for fetching</param>
    /// <param name="mediaType"></param>
    /// <remarks>Fetch all asset categories </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetAssetCategoriesAsync")]
    [ValidateModel]
    public async Task<IActionResult> GetAssetCategoriesAsync(
        [FromQuery] GetAssetCategoriesResourceParameters parameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {

        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        if (!string.IsNullOrEmpty(parameters.OrderBy)
        && !_propertyMappingService.ValidMappingExistsFor<AssetCategoryUiModel, AssetCategory>(parameters.OrderBy))
        {
            return this.BadRequest("ASSET_CATEGORIES_MODEL_ERROR");
        }

        if (!_typeHelperService.TypeHasProperties<AssetCategoryUiModel>
            (parameters.Fields))
        {
            return this.BadRequest("ASSET_CATEGORIES_FIELDS_ERROR");
        }
        BusinessResult<PagedList<AssetCategoryUiModel>> fetchedAssetCategories;
        try
        {
            Log.Information(
                $"--Method:GetAssetCategoriesAsync -- Message:ASSET_CATEGORIES_FETCH" +
                $" -- Datetime:{DateTime.UtcNow} -- Just Before : GetAssetCategoriesAsync");
            fetchedAssetCategories = await _mediator.Send(new GetAssetCategoriesQuery(parameters));
            Log.Information(
                $"--Method:GetAssetCategoriesAsync -- Message:ASSET_CATEGORIES_FETCH" +
                $" -- Datetime:{DateTime.UtcNow} -- Just After : GetAssetCategoriesAsync");
        }
        catch (Exception e)
        {
            Log.Error(
                $"--Method:GetAssetCategoriesAsync -- Message:ASSET_CATEGORIES_FETCH_ERROR" +
                $" -- Datetime:{DateTime.UtcNow} -- AssetCategoryInfo:{e.Message} - Details :  {e.InnerException?.Message}");
            return this.BadRequest("ERROR_FETCH_ASSET_CATEGORIES");
        }


        if (fetchedAssetCategories.IsNull())
        {
            return NotFound("ASSET_CATEGORIES_NOT_FOUND");
        }

        if (!fetchedAssetCategories.IsSuccess())
            return OkOrBadRequest(fetchedAssetCategories.BrokenRules);

        if (fetchedAssetCategories.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(fetchedAssetCategories.BrokenRules);

        var responseWithMetaData = CreateOkWithMetaData(fetchedAssetCategories.Model, mediaType,
            parameters, _urlHelper, "GetAssetCategoryByIdAsync", "GetAssetCategoriesAsync");
        return Ok(responseWithMetaData);
    }

    /// <summary>
    /// Get - Fetch one asset category
    /// </summary>
    /// <param name="id">AssetCategory Id for fetching</param>
    /// <remarks>Fetch one asset category </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("{id}", Name = "GetAssetCategoryByIdAsync")]
    [ValidateModel]
    public async Task<IActionResult> GetAssetCategoryByIdAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var fetchedAssetCategory = await _mediator.Send(new GetAssetCategoryByIdQuery(id));

        if (fetchedAssetCategory.IsNull())
        {
            return NotFound("ASSET_CATEGORY_NOT_FOUND");
        }

        if (!fetchedAssetCategory.IsSuccess())
            return OkOrBadRequest(fetchedAssetCategory.BrokenRules);

        if (fetchedAssetCategory.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(fetchedAssetCategory.BrokenRules);

        return Ok(fetchedAssetCategory);
    }

    /// <summary>
    /// Post - Create an asset category
    /// </summary>
    /// <param name="request">CreateAssetCategoryResourceParameters for creation</param>
    /// <remarks>Create asset category </remarks>
    /// <response code="200">Resource Creation Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPost(Name = "PostAssetCategoryAsync")]
    [ValidateModel]
    public async Task<IActionResult> PostAssetCategoryAsync([FromBody] CreateAssetCategoryResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var createAssetCategory = await _mediator.Send(
            new CreateAssetCategoryCommand(userAudit.Model.Id, request.Name, request.CodeErp, request.Params)
        );

        if (createAssetCategory.IsNull())
        {
            return NotFound("ERROR_ASSET_CATEGORY_CREATED_NOT_FOUND");
        }

        if (!createAssetCategory.IsSuccess())
            return OkOrBadRequest(createAssetCategory.BrokenRules);

        if (createAssetCategory.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(createAssetCategory.BrokenRules);

        return Ok(createAssetCategory);
    }

    /// <summary>
    /// Put - Update an asset category
    /// </summary>
    /// <param name="id">AssetCategory Id for modification</param>
    /// <param name="request">UpdateAssetCategoryResourceParameters for modification</param>
    /// <remarks>Update asset category </remarks>
    /// <response code="200">Resource Modification Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpPut("{id}", Name = "UpdateAssetCategoryAsync")]
    [ValidateModel]
    public async Task<IActionResult> UpdateAssetCategoryAsync(long id, [FromBody] UpdateAssetCategoryResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        var response = await _mediator.Send(new UpdateAssetCategoryCommand(userAudit.Model.Id, id, request.Name, request.CodeErp, request.Params));

        if (response.IsNull())
        {
            return NotFound("ASSET_CATEGORY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing asset category - Soft Delete
    /// </summary>
    /// <param name="id">AssetCategory Id for deletion</param>
    /// <param name="request">DeleteAssetCategoryResourceParameters for deletion</param>
    /// <remarks>Delete existing asset category </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("soft/{id}", Name = "DeleteSoftAssetCategoryAsync")]
    [ValidateModel]
    public async Task<IActionResult> DeleteSoftAssetCategoryAsync(long id, [FromBody] DeleteAssetCategoryResourceParameters request)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }

        var response = await _mediator.Send(new DeleteSoftAssetCategoryCommand(id, userAudit.Model.Id, request.DeletedReason));

        if (response.IsNull())
        {
            return NotFound("ASSET_CATEGORY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }

    /// <summary>
    /// Delete - Delete an existing asset category - Hard Delete
    /// </summary>
    /// <param name="id">Comapny Id for deletion</param>
    /// <remarks>Delete existing asset category </remarks>
    /// <response code="200">Resource Deletion Finished</response>
    /// <response code="204">Resource No Content</response>
    /// <response code="401">Resource Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardAssetCategoryRoot")]
    [ValidateModel]
    [Authorize(Roles = "SU")]
    public async Task<IActionResult> DeleteHardAssetCategoryAsync(long id)
    {
        var userAudit = await this._mediator.Send(new GetPersonByEmailQuery(this.GetEmailFromClaims()));

        if (userAudit.IsNull())
        {
            return this.NotFound("USER_AUDIT_NOT_FOUND");
        }
        if (!this.IsSuFromClaims())
        {
            Log.Error(
              $"--Method:DeleteHardAssetCategoryRoot -- Message:ASSET_CATEGORIES_HARD_DELETION" +
              $" -- Datetime:{DateTime.UtcNow} -- Action: DeleteHardAssetCategoryAsync");
            return this.BadRequest("ERROR_NOT_AUTHORIZED_FOR_HARD_DELETION");
        }
        var response = await _mediator.Send(new DeleteHardAssetCategoryCommand(id));

        if (response.IsNull())
        {
            return NotFound("ASSET_CATEGORY_NOT_FOUND");
        }

        if (!response.IsSuccess())
            return OkOrBadRequest(response.BrokenRules);

        if (response.Status == BusinessResultStatus.Fail)
            return OkOrNoResult(response.BrokenRules);

        return Ok(response);
    }
}//Class: AssetCategoriesController
