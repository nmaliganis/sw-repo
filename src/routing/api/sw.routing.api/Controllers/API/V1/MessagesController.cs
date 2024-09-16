using sw.infrastructure.Controlles.Base;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace sw.routing.api.Controllers.API.V1;

/// <summary>
/// Messages
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class MessagesController : BaseController
{
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;


    /// <summary>
    /// Ctor : PingController
    /// </summary>
    /// <param name="urlHelper"></param>
    /// <param name="typeHelperService"></param>
    /// <param name="propertyMappingService"></param>
    public MessagesController(IUrlHelper urlHelper,
        ITypeHelperService typeHelperService,
        IPropertyMappingService propertyMappingService
    )
    {
        this._urlHelper = urlHelper;
        this._typeHelperService = typeHelperService;
        this._propertyMappingService = propertyMappingService;

    }

    /// <summary>
    /// Ping - Pong
    /// </summary>
    /// <returns></returns>
    [HttpGet("ping", Name = "PingRoot")]
    public async Task<IActionResult> PingAsync()
    {
        return this.Ok();
    }
}