using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.ResourceParameters.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using sw.admin.api.Validators;
using sw.admin.common.dtos.V1.ResourceParameters.Companies;

namespace sw.admin.api.Controllers.API.V1
{
    /// <summary>
    /// Company Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
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
        [HttpGet(Name = "GetCompaniesAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetCompaniesAsync(
            [FromQuery] GetCompaniesResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy)
            && !_propertyMappingService.ValidMappingExistsFor<CompanyUiModel, Company>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<CompanyUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetCompaniesQuery(parameters));

            if (response == null)
            {
                return NotFound();
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
        [HttpGet("{id}", Name = "GetCompanyByIdAsync")]
        [ValidateModel]
        public async Task<IActionResult> GetCompanyByIdAsync(long id)
        {
            var response = await _mediator.Send(new GetCompanyByIdQuery(id));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return Ok(response);
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
        [HttpPost(Name = "PostCompanyAsync")]
        [ValidateModel]
        public async Task<IActionResult> PostCompanyAsync([FromBody] CreateCompanyResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateCompanyCommand(1, request.Name, request.CodeErp, request.Description)
            );

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return this.CreatedOrNoResult(response);
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
        [HttpPut("{id}", Name = "UpdateCompanyAsync")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCompanyAsync(long id, [FromBody] UpdateCompanyResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateCompanyCommand(2, id, request.Name, request.CodeErp, request.Description));

            if (response == null)
            {
                return NotFound();
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
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteSoftCompanyCommand(id, deletedBy, request.DeletedReason));

            if (response == null)
            {
                return NotFound();
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
        [HttpDelete("hard/{id}", Name = "DeleteHardCompanyAsync")]
        [ValidateModel]
        //[Authorize(Roles = "SU")]
        public async Task<IActionResult> DeleteHardCompanyAsync(long id)
        {
            var response = await _mediator.Send(new DeleteHardCompanyCommand(id));

            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess())
                return OkOrBadRequest(response.BrokenRules);

            if (response.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(response.BrokenRules);

            return Ok(response);
        }
    }
}
