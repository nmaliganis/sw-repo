using System;
using System.Net;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace sw.auth.api.Controllers.API.V1
{
    /// <summary>
    /// Class : MessagesController
    /// </summary>
    [ApiVersionNeutral]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IBus _bus;

        /// <summary>
        /// Constructor : MessagesController
        /// </summary>
        /// <param name="bus"></param>
        public MessagesController(IBus bus)
        {
            _bus = bus;
        }


        /// <summary>
        /// POST PostMessageRoot
        /// </summary>
        /// <remarks> return the current user </remarks>
        /// <response code="200">200 (OK) </response>
        /// <response code="400">400 (Bad Request)</response>
        /// <response code="500">500 (Internal Server Error) </response>
        [Route("/propagate", Name = "PostMessageRoot")]
        [HttpPost]
        public async Task<IActionResult> PostMessageAsync()
        {
            Log.Information(" On Register message ");
            try
            {
                //await _bus.Publish<AuthRegistration>( new
                //{
                //    CorrelationId = Guid.NewGuid(),
                //    Title = "New Message"
                //});

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred during .." + ex.Message);
                return BadRequest();
            }
        }
    }
}