using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Links;
using sw.localization.common.dtos.ResourceParameters.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.common.Exceptions;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Helpers;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Linq;
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
    public class LocalizationValuesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="urlHelper"></param>
        /// <param name="typeHelperService"></param>
        /// <param name="propertyMappingService"></param>
        public LocalizationValuesController(
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
        /// Get - Retrieve Stored Localization Value by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetLocalizationValueById")]
        [ValidateModel]
        public async Task<IActionResult> GetLocalizationValueByIdAsync(long id)
        {
            //if (!_typeHelperService.TypeHasProperties<RoleUiModel>
            //    (fields))
            //{
            //    return BadRequest();
            //}

            var localizationValue = await _mediator.Send(new GetLocalizationValueByIdQuery(id));

            if (localizationValue == null)
            {
                return NotFound();
            }

            return Ok(localizationValue);
        }

        /// <summary>
        /// Get - Retrieve Stored Localization Value providing Key - Domain - Lang
        /// </summary>
        /// <param name="key">Localization Key</param>
        /// <param name="domain">Localization Domain</param>
        /// <param name="lang">Localization Lang</param>
        /// <remarks>Retrieve Containers providing Id and [Optional] fields</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{key}/domain/{domain}/language/{lang}", Name = "GetLocalizationValueByKeyAndDomainAndLang")]
        [ValidateModel]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLocalizationValueByKeyAndDomainAndLangAsync(string key, string domain, string lang)
        {
            var localizationValue = await _mediator.Send(new GetLocalizationValueByKeyQuery(key, domain, lang));

            if (localizationValue == null)
            {
                return NotFound();
            }

            if (!localizationValue.IsSuccess())
                return OkOrBadRequest(localizationValue.BrokenRules);

            if (localizationValue.Status == BusinessResultStatus.Fail)
                return OkOrNoResult(localizationValue.BrokenRules);

            return Ok(localizationValue);
        }

        /// <summary>
        /// Get - Retrieve Stored Localization Values providing Domain - Lang
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="lang"></param>
        /// <param name="parameters"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        [HttpGet("domain/{domain}/language/{lang}", Name = "GetLocalizationValues")]
        [ValidateModel]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLocalizationValuesAsync(string domain, string lang,
            [FromQuery] GetLocalizationValuesResourceParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<LocalizationValueUiModel, LocalizationValue>
                (parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<LocalizationValueUiModel>
                (parameters.Fields))
            {
                return BadRequest();
            }

            var allQry = new GetLocalizationValuesQuery(parameters);
            allQry.Domain = domain;
            allQry.Language = lang;

            var localizationValues = await _mediator.Send(allQry);

            var items = localizationValues as IEnumerable<LocalizationValueUiModel>;

            if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
            {
                var paginationMetadata = new
                {
                    totalCount = localizationValues.TotalCount,
                    pageSize = localizationValues.PageSize,
                    currentPage = localizationValues.CurrentPage,
                    totalPages = localizationValues.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForLocalizationValues(parameters,
                    localizationValues.HasNext, localizationValues.HasPrevious);

                var shapedLocalizationValues = items.ShapeData(parameters.Fields);

                var shapedLocalizationValuesWithLinks = shapedLocalizationValues.Select(locValue =>
                {
                    var locValuesAsDictionary = locValue as IDictionary<string, object>;
                    var locValuesLinks =
                        CreateLinksForLocalizationValue((long)locValuesAsDictionary["Id"], parameters.Fields);

                    locValuesAsDictionary.Add("links", locValuesLinks);

                    return locValuesAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedLocalizationValuesWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = localizationValues.HasPrevious
                    ? CreateLocalizationValuesResourceUri(parameters,
                        ResourceUriType.PreviousPage)
                    : null;

                var nextPageLink = localizationValues.HasNext
                    ? CreateLocalizationValuesResourceUri(parameters,
                        ResourceUriType.NextPage)
                    : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = localizationValues.TotalCount,
                    pageSize = localizationValues.PageSize,
                    currentPage = localizationValues.CurrentPage,
                    totalPages = localizationValues.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));

                return Ok(items.ShapeData(parameters.Fields));
            }

            //return Ok(localizationValues);
        }

        /// <summary>
        /// Method : PostLocalizationValueAsync
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostLocalizationValue")]
        [ValidateModel]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostLocalizationValueAsync([FromBody] CreateLocalizationValueResourceParameters request)
        {
            var response = await _mediator.Send(
                new CreateLocalizationValueCommand(request.Key, request.Value, request.Domain, request.Language)
            );

            /*return response.Match<IActionResult>(
                localizationValueUiModel =>
                {
                    return CreatedAtAction(
                        nameof(GetLocalizationValuesAsync),
                        new { domain = request.Domain, language = request.Language, key = request.Key },
                        response.AsT0);
                },
                Exception =>
                {
                    Log.Error(
                      $"Create Localization: {request.Key} - {request.Value}" +
                      $"Error Message:{response.AsT1.Message}" +
                      $"--CreateLocalization--  @fail@ [{nameof(ICreateLocalizationValueProcessor)}]. " +
                      $"@inner-fault:{response.AsT1.InnerException}");

                    switch (Exception)
                    {
                        case LocalizationDomainDoesNotExistException:
                        case LocalizationLanguageDoesNotExistException:
                        case LocalizationKeyAlreadyExistsException:
                            return NotFound(response.AsT1);
                        default:
                            return BadRequest(response.AsT1);
                    }
                }
           );*/
            return Ok();
        }

        /// <summary>
        /// Put - Update an existing Localization Value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "UpdateLocalizationValue")]
        [ValidateModel]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateLocalizationValueAsync(long id, [FromBody] UpdateLocalizationValueResourceParameters request)
        {
            var response = await _mediator.Send(new UpdateLocalizationValueCommand(id, request.Value));

            return response.Match<IActionResult>(
                LocalizationValueUiModel => { return Ok(response.AsT0); },
                Exception =>
                {
                    Log.Error(
                      $"Update Localization Id: {id} - {request.Value}" +
                      $"Error Message:{response.AsT1.Message}" +
                      $"--UpdateLocalization--  @fail@ [{nameof(IUpdateLocalizationValueProcessor)}]. " +
                      $"@inner-fault:{response.AsT1.InnerException}");

                    switch (Exception)
                    {
                        case LocalizationIdDoesNotExistException:
                            return NotFound(response.AsT1);
                        default:
                            return BadRequest(response.AsT1);
                    }
                }
            );
        }

        /// <summary>
        /// Delete - Delete an existing Localization Value - Soft Delete
        /// </summary>
        /// <param name="id">Localization Value Id for Deletion</param>
        /// <param name="request">DeleteLocalizationValueResourceParameters for Deletion</param>
        /// <remarks>Delete Existing Localization Value </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("{id}", Name = "DeleteLocalizationValueAsync")]
        [ValidateModel]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLocalizationValueAsync(long id, [FromBody] DeleteLocalizationValueResourceParameters request)
        {
            long deletedBy = 2; // TODO: Get user from Auth
            var response = await _mediator.Send(new DeleteLocalizationValueCommand(id, deletedBy, request.DeletedReason));

            return response.Match<IActionResult>(
                LocalizationValueUiModel => { return Ok(response.AsT0); },
                Exception =>
                {
                    Log.Error(
                      $"Soft Delete Localization: {id}" +
                      $"Error Message:{response.AsT1.Message}" +
                      $"--DeleteLocalization--  @fail@ [{nameof(IUpdateLocalizationValueProcessor)}]. " +
                      $"@inner-fault:{response.AsT1.InnerException}");

                    switch (Exception)
                    {
                        case LocalizationIdDoesNotExistException:
                            return NotFound(response.AsT1);
                        default:
                            return BadRequest(response.AsT1);
                    }
                }
            );
        }

        /// <summary>
        /// Delete - Delete an existing Localization Value - Hard Delete
        /// </summary>
        /// <param name="id">Localization Value Id for Deletion</param>
        /// <remarks>Delete Existing Localization Value </remarks>
        /// <response code="200">Resource Deletion Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("hard/{id}", Name = "HardDeleteLocalizationValueAsync")]
        [ValidateModel]
        [Authorize(Roles = "SU")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> HardDeleteLocalizationValueAsync(long id)
        {
            var response = await _mediator.Send(new HardDeleteLocalizationValueCommand(id));

            return response.Match<IActionResult>(
                LocalizationValueUiModel => { return Ok(response.AsT0); },
                Exception =>
                {
                    Log.Error(
                      $"Hard Delete Localization: {id}" +
                      $"Error Message:{response.AsT1.Message}" +
                      $"--DeleteLocalization--  @fail@ [{nameof(IUpdateLocalizationValueProcessor)}]. " +
                      $"@inner-fault:{response.AsT1.InnerException}");

                    switch (Exception)
                    {
                        case LocalizationIdDoesNotExistException:
                            return NotFound(response.AsT1);
                        default:
                            return BadRequest(response.AsT1);
                    }
                }
            );
        }



        #region Link Builder

        private IEnumerable<LinkDto> CreateLinksForLocalizationValue(long id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetLocalizationValueById", new { id = id }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetLocalizationValueById", new { id = id, fields = fields }),
                        "self",
                        "GET"));
            }

            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForLocalizationValues(GetLocalizationValuesResourceParameters parameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateLocalizationValuesResourceUri(parameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateLocalizationValuesResourceUri(parameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateLocalizationValuesResourceUri(parameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateLocalizationValuesResourceUri(GetLocalizationValuesResourceParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetLocalizationValues",
                        new
                        {
                            fields = parameters.Fields,
                            orderBy = parameters.OrderBy,
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageIndex - 1,
                            pageSize = parameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetLocalizationValues",
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
                    return _urlHelper.Link("GetLocalizationValues",
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

        #endregion
    }
}