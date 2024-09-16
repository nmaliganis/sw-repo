using sw.logging.api.Helpers;
using sw.logging.api.Models;
using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace sw.logging.api.Configurations.Installers;

/// <summary>
/// Class
/// </summary>
public static class MartenInstaller
{
    /// <summary>
    /// Method : AddMarten
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    public static void AddMarten(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(CreateDocumentStore(connectionString));
        services.AddScoped<IDataStore, DataStore>();
    }

    private static IDocumentStore CreateDocumentStore(string cn)
    {
        return DocumentStore.For(_ =>
        {
            _.Connection(cn);
            _.Logger(new LogMartenLogger());

            _.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            _.Schema.For<Log>().DocumentAlias("log_store").Duplicate(
                t => t.Correlation, pgType: "uuid", configure: idx => idx.IsUnique = true);
            _.DatabaseSchemaName = "log_store";

            _.Serializer(CustomizeJsonSerializer());
            _.Events.DatabaseSchemaName = "event_store";
        });
    }

    private static JsonNetSerializer CustomizeJsonSerializer()
    {
        var serializer = new JsonNetSerializer();

        serializer.Customize(_ =>
        {
            _.ContractResolver = new ProtectedSettersContractResolver();
        });

        return serializer;
    }
}