using System;
using System.Threading.Tasks;
using sw.logging.api.Models;

namespace sw.logging.api.Repositories;

/// <summary>
/// Interface : ILogRepository
/// </summary>
public interface ILogRepository
{
    /// <summary>
    /// Method
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    Task<Log> WithId(Guid guid);

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="id"></param>
    Task<Log> this[Guid id] { get; }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="log"></param>
    void Add(Log log);

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Exists(Guid id);
}