using sw.interprocess.api.Commanding.Servers;
using sw.interprocess.api.Models.Requests;
using sw.interprocess.api.Models.Responses;
using sw.interprocess.api.Validators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace sw.interprocess.api.Controllers
{
    /// <summary>
    /// Interprocess Controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SU, ADMIN")]
    public class InterprocessController : ControllerBase
    {
        /// <summary>
        /// Post -Parser Message
        /// </summary>
        /// <param name="payload"></param>
        /// <response code="201">Resource Post Finished</response>
        /// <response code="204">Resource No Content</response>
        /// <response code="401">Resource Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPost("parser", Name = "PostParserRoot")]
        [ValidateModel]
        [ProducesResponseType(typeof(WsPostBroadcastResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostParserAsync([FromBody] byte[] payload)
        {
            return Ok();
        }


        [HttpGet(Name = "GetInterprocessVersionRoot")]
        [ValidateModel]
        public async Task<IActionResult> GetInterprocessVersionAsync()
        {
            return this.Ok("V6");
        }

    }//Class : InterprocessController
}