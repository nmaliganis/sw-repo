using System;
using System.Net;
using System.Threading.Tasks;
using sw.logging.api.Helpers;
using sw.logging.api.Models;
using sw.logging.api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace sw.logging.api.Controllers;

/// <summary>
/// Logs Controller
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    /// <summary>
    /// Property
    /// </summary>
    public IDataStore DataStore { get; }

    /// <summary>
    /// Ctor
    /// </summary>
    public LogsController(IDataStore dataStore)
    {
        this.DataStore = dataStore;
    }

    /// <summary>
    /// Method : GetLogsAsync
    /// </summary>
    /// <returns></returns>
    [HttpGet("{idLog:Guid}", Name = "GetLogsRoot")]
    public async Task<IActionResult> GetLogsAsync(Guid idLog)
    {
        return this.Ok();
    }


    /// <summary>
    /// Method : GetLogsAsync
    /// </summary>
    /// <returns></returns>
    [HttpPost(Name = "PostLogsRoot")]
    public async Task<IActionResult> PostLogsAsync([FromBody] Log newLog)
    {
        ILogRepository logRepository = new LogRepository(this.DataStore.Session);

        logRepository.Add(newLog);

        try
        {
            await this.DataStore.CommitChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return this.Ok();
    }

}//Class : LogsController