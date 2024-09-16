using System;
using System.Threading.Tasks;
using sw.logging.api.Repositories;
using Marten;

namespace sw.logging.api.Helpers;

/// <summary>
/// Class : DataStore
/// </summary>
public class DataStore : IDataStore
{
    /// <summary>
    /// Property : Session
    /// </summary>
    public IDocumentSession Session { get; set; }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="documentStore"></param>
    public DataStore(IDocumentStore documentStore)
    {
        Session = documentStore.LightweightSession();
    }

    /// <summary>
    /// Method : CommitChanges
    /// </summary>
    public async Task CommitChanges()
    {
        await Session.SaveChangesAsync();
    }

    /// <summary>
    /// Method : Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Method : Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Session.Dispose();
        }
    }
}