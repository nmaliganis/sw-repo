using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Domain;
using sw.infrastructure.Extensions;
using sw.infrastructure.Helpers;
using sw.infrastructure.Helpers.Controllers;
using sw.infrastructure.Paging;
using sw.infrastructure.ResourseParameters;
using sw.infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace sw.infrastructure.Controlles.Base
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
            if (claimsPrincipal.Claims.Count() >= 0)
            {
                var email = claimsPrincipal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                    ?.Value;
                return email;
            }

            return String.Empty;
        }

        /// <summary>
        /// GetMemberFromClaims
        /// </summary>
        /// <returns></returns>
        protected long GetMemberFromClaims()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var userData = claimsPrincipal?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata")
                ?.Value;
            long memberId = -1;

            if (!String.IsNullOrEmpty(userData))
            {
                var authUserDataUiModel = userData.JsonToObject<AuthUserDataUiModel>();

                memberId = authUserDataUiModel.MemberId;
            }

            return memberId;
        }

        /// <summary>
        /// Method : GetCompanyFromClaims
        /// </summary>
        /// <returns></returns>
        protected long GetCompanyFromClaims()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var userData = claimsPrincipal?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata")
                ?.Value;

            long companyId = -1;

            if (!String.IsNullOrEmpty(userData))
            {
                var authUserDataUiModel = userData.JsonToObject<AuthUserDataUiModel>();

                companyId = authUserDataUiModel.CompanyId;
            }

            return companyId;
        }

        /// <summary>
        /// GetEmailFromClaims
        /// </summary>
        /// <returns></returns>
        protected bool IsSuFromClaims()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var isSu = ((claimsPrincipal?.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")) ?? Array.Empty<Claim>())
                .FirstOrDefault(x => x.Value == "SU")
                ?.Value;
            return !String.IsNullOrEmpty(isSu);
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
        /// CreatedOrNoResult
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected IActionResult CreatedOrNoResult(object input)
        {
            if (!input.IsNull())
                return Created(nameof(input), input);

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
            var links = new List<LinkDto> {
                // self
                Url.LinkDto("self", "GET", nameOfAction, routerParameters)};

            if (hasNext)
                links.Add(Url.LinkDto("nextPage", "GET", nameOfAction, routerParameters));

            if (hasPrevious)
                links.Add(Url.LinkDto("previousPage", "GET", nameOfAction, routerParameters));

            return links;
        }

        /// <summary>
        /// CreateLinksFor
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="routeName"></param>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected IEnumerable<LinkDto> CreateLinksFor(IUrlHelper urlHelper, string routeName, long id, string fields)
        {
            var links = new List<LinkDto>();

            links.Add(string.IsNullOrWhiteSpace(fields)
                ? Url.LinkDto(urlHelper.Link(routeName, new { id = id }), "self", "GET")
                : Url.LinkDto(urlHelper.Link(routeName, new { id = id, fields = fields }), "self", "GET"));

            return links;
        }

        /// <summary>
        /// CreateLinksForList
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="routeName"></param>
        /// <param name="parameters"></param>
        /// <param name="hasNext"></param>
        /// <param name="hasPrevious"></param>
        /// <returns></returns>
        protected IEnumerable<LinkDto> CreateLinksForList(IUrlHelper urlHelper, string routeName, BaseResourceParameters parameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateResourceUri(urlHelper, routeName, parameters, ResourceUriType.Current), "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateResourceUri(urlHelper, routeName, parameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateResourceUri(urlHelper, routeName, parameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }

            return links;
        }

        /// <summary>
        /// CreateResourceUri
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="routeName"></param>
        /// <param name="parameters"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string CreateResourceUri(IUrlHelper urlHelper, string routeName, BaseResourceParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link(routeName,
                        new
                        {
                            fields = parameters.Fields,
                            orderBy = parameters.OrderBy,
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageIndex - 1,
                            pageSize = parameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link(routeName,
                        new
                        {
                            fields = parameters.Fields,
                            orderBy = parameters.OrderBy,
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageIndex + 1,
                            pageSize = parameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return urlHelper.Link(routeName,
                        new
                        {
                            fields = parameters.Fields,
                            orderBy = parameters.OrderBy,
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageIndex,
                            pageSize = parameters.PageSize
                        });
            }
        }

        /// <summary>
        /// CreateOkWithMetaData
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pagedResponse"></param>
        /// <param name="mediaType"></param>
        /// <param name="parameters"></param>
        /// <param name="urlHelper"></param>
        /// <param name="routeNameForSingle"></param>
        /// <param name="routeNameForList"></param>
        /// <returns></returns>
        protected OkObjectResult CreateOkWithMetaData<TEntity>(PagedList<TEntity> pagedResponse, string mediaType, BaseResourceParameters parameters, IUrlHelper urlHelper, string routeNameForSingle, string routeNameForList)
        {
            var items = pagedResponse as IEnumerable<TEntity>;

            if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
            {
                var paginationMetadata = new
                {
                    totalCount = pagedResponse.TotalCount,
                    pageSize = pagedResponse.PageSize,
                    currentPage = pagedResponse.CurrentPage,
                    totalPages = pagedResponse.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForList(urlHelper, routeNameForList, parameters, pagedResponse.HasNext, pagedResponse.HasPrevious);

                var shapedResponse = items.ShapeData(parameters.Fields);

                var shapedResponseWithLinks = shapedResponse.Select(resp =>
                {
                    var respAsDictionary = resp as IDictionary<string, object>;
                    var respLinks = CreateLinksFor(urlHelper, routeNameForSingle, (long)respAsDictionary["Id"], parameters.Fields);

                    respAsDictionary.Add("links", respLinks);

                    return respAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedResponseWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = pagedResponse.HasPrevious
                    ? CreateResourceUri(urlHelper, routeNameForList, parameters, ResourceUriType.PreviousPage)
                    : null;

                var nextPageLink = pagedResponse.HasNext
                    ? CreateResourceUri(urlHelper, routeNameForList, parameters, ResourceUriType.NextPage)
                    : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = pagedResponse.TotalCount,
                    pageSize = pagedResponse.PageSize,
                    currentPage = pagedResponse.CurrentPage,
                    totalPages = pagedResponse.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));

                return Ok(items.ShapeData(parameters.Fields));
            }
        }
    }
}