using sw.localization.common.dtos.Cqrs.LocalizationLanguages;
using sw.localization.common.dtos.ResourceParameters.LocalizationLanguages;
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
    public class LocalizationLanguagesController : BaseController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mediator"></param>
        public LocalizationLanguagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Post - Create a Localization Language
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostLocalizationLanguage")]
        [ValidateModel]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostLocalizationLanguageAsync([FromBody] CreateLocalizationLanguageResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateLocalizationLanguageCommand(request.Language)
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
        /// Delete - Delete an existing Localization Language - Hard Delete
        /// </summary>
        /// <param name="id">Localization Language Id for Deletion</param>
        /// <remarks>Delete Existing Localization Language </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "HardDeleteLocalizationLanguageAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> HardDeleteLocalizationLanguageAsync(long id)
        {
            var response = await _mediator.Send(new HardDeleteLocalizationLanguageCommand(id));

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
