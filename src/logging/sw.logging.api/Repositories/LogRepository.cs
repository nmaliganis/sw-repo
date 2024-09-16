using System;
using System.Threading.Tasks;
using Marten;
using Serilog;
using Log = sw.logging.api.Models.Log;

namespace sw.logging.api.Repositories;

/// <summary>
/// Class
/// </summary>
public class LogRepository : ILogRepository
{
    private readonly IDocumentSession _session;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="session"></param>
    public LogRepository(IDocumentSession session)
    {
        _session = session;
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Log> WithId(Guid id)
    {
        return await _session.Query<Log>().FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotImplementedException"></exception>
    public Task<Log> this[Guid id] => throw new NotImplementedException();

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="log"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Add(Log log)
    {
        _session.Insert(log);
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Exists(Guid id)
    {
        return await _session.Query<Log>().AnyAsync(t => t.Id == id);
    }
}