using System;
using System.Linq;
using Marten;
using Marten.Services;
using Npgsql;

namespace sw.logging.api.Helpers;

/// <summary>
/// Class
/// </summary>
public class LogMartenLogger : IMartenLogger, IMartenSessionLogger
{
    /// <summary>
    /// Method
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public IMartenSessionLogger StartSession(IQuerySession session)
    {
        return this;
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="sql"></param>
    public void SchemaChange(string sql)
    {
        Console.WriteLine("Executing schema change with the following DDL:");
        Console.WriteLine(sql);
        Console.WriteLine();
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="command"></param>
    public void LogSuccess(NpgsqlCommand command)
    {
        Console.WriteLine($"CommandText={command.CommandText}");

        Console.WriteLine("Parameters");
        foreach (NpgsqlParameter parameter in command.Parameters)
        {
            Console.WriteLine($"Parameter '{parameter.ParameterName}' =  '{parameter.Value}'");
        }
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="command"></param>
    /// <param name="ex"></param>
    public void LogFailure(NpgsqlCommand command, Exception ex)
    {
        Console.WriteLine("Postgresql command failed");
        Console.WriteLine(command.CommandText);
        Console.WriteLine(ex);
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="session"></param>
    /// <param name="commit"></param>
    public void RecordSavedChanges(IDocumentSession session, IChangeSet commit)
    {
        var lastCommit = commit;
        Console.WriteLine(
            $"Persisted {lastCommit.Updated.Count()} updates, {lastCommit.Inserted.Count()} inserts, and {lastCommit.Deleted.Count()} deletions");
    }

    /// <summary>
    /// Method
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnBeforeExecute(NpgsqlCommand command)
    {
        Console.WriteLine(command.CommandText);
    }
}