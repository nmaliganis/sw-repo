using System;
using System.Threading.Tasks;
using sw.logging.api.Repositories;
using Marten;

namespace sw.logging.api.Helpers;

/// <summary>
/// Interface : IDataStore
/// </summary>
public interface IDataStore : IDisposable
{
    /// <summary>
    /// Method : CommitChanges
    /// </summary>
    /// <returns></returns>
    Task CommitChanges();

    /// <summary>
    /// Property : Session
    /// </summary>
    IDocumentSession Session { get; set; }
}