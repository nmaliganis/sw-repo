using sw.localization.common.dtos.Cqrs.LocalizationDomains;
using sw.localization.common.dtos.ResourceParameters.LocalizationDomains;
using sw.infrastructure.BrokenRules;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using sw.localization.api.Controllers.API.Base;
using sw.localization.api.Validators;

namespace sw.localization.web.api.Controllers.API.V1
{
    /// <summary>
    /// Class
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class LocalizationDomainsController : BaseController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mediator"></param>
        public LocalizationDomainsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Post - Create a Localization Domain
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostLocalizationDomain")]
        [ValidateModel]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostLocalizationDomainAsync([FromBody] CreateLocalizationDomainResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateLocalizationDomainCommand(request.Domain)
            );

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
        /// Delete - Delete an existing Localization Domain - Hard Delete
        /// </summary>
        /// <param name="id">Localization Domain Id for Deletion</param>
        /// <remarks>Delete Existing Localization Domain </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "HardDeleteLocalizationDomainAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> HardDeleteLocalizationDomainAsync(long id)
        {
            var response = await _mediator.Send(new HardDeleteLocalizationDomainCommand(id));

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
