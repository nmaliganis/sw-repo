using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using sw.localization.api.Helpers.Models;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Domain;
using sw.infrastructure.Extensions;
using sw.infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace sw.localization.api.Controllers.API.Base
{
    /// <summary>
    /// BaseController
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// GetEmailFromClaims
        /// </summary>
        /// <returns></returns>
        protected string GetEmailFromClaims()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var email = claimsPrincipal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                .Value;
            return email;
        }

        private IActionResult BadRequestSerialized(BusinessResult businessResult) =>
                    BadRequest(new ErrorResponse(businessResult.BrokenRules.Select(x => x.Rule)));

        /// <summary>
        /// OkOrBadRequest
        /// </summary>
        /// <param name="businessResult"></param>
        /// <returns></returns>
        protected IActionResult OkOrBadRequest(BusinessResult businessResult)
        {
            return !businessResult.IsSuccess() ? BadRequestSerialized(businessResult) : Ok();
        }
        /// <summary>
        /// OkOrNoResult
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected IActionResult OkOrNoResult(object input)
        {
            if (!input.IsNull())
                return Ok(input);

            return NoContent();
        }
        /// <summary>
        /// OkOrBadRequest
        /// </summary>
        /// <param name="businessResult"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected IActionResult OkOrBadRequest(BusinessResult businessResult, object model)
        {
            return !businessResult.IsSuccess() ? BadRequestSerialized(businessResult) : Ok(model);
        }
        /// <summary>
        /// OkOrBadRequest
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="businessResult"></param>
        /// <returns></returns>
        protected IActionResult OkOrBadRequest<T>(BusinessResult<T> businessResult) where T : new()
        {
            return !businessResult.IsSuccess() ? BadRequestSerialized(businessResult) : Ok(businessResult.Model);
        }

        /// <summary>
        /// NoContentOrBadRequest
        /// </summary>
        /// <param name="businessResult"></param>
        /// <returns></returns>
        protected IActionResult NoContentOrBadRequest(BusinessResult businessResult)
        {
            return !businessResult.IsSuccess() ? BadRequestSerialized(businessResult) : NoContent();
        }

        /// <summary>
        /// CreateLinksForList
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="nameOfAction"></param>
        /// <param name="routerParameters"></param>
        /// <param name="hasNext"></param>
        /// <param name="hasPrevious"></param>
        /// <returns></returns>
        protected IEnumerable<LinkDto> CreateLinksForList<TEntity>(string nameOfAction, object routerParameters,
                bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self
            links.Add(Url.LinkDto("self", "GET", nameOfAction, routerParameters));

            if (hasNext)
                links.Add(Url.LinkDto("nextPage", "GET", nameOfAction, routerParameters));

            if (hasPrevious)
                links.Add(Url.LinkDto("previousPage", "GET", nameOfAction, routerParameters));

            return links;
        }
    }
}