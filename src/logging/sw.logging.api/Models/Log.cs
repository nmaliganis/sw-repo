using System;
using System.Threading.Tasks;

namespace sw.logging.api.Models;

/// <summary>
/// Class
/// </summary>
public class Log
{
    /// <summary>
    /// Ctor
    /// </summary>
    public Log(Guid correlation, string serviceName, SeverityType type, string message)
    {
        this.ServiceName = serviceName;
        this.Type = type;
        this.Message = message;

        this.Id = Guid.NewGuid();
        this.Received = DateTime.UtcNow;
        this.Correlation = correlation;
    }

    /// <summary>
    /// Property : Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Property : Correlation
    /// </summary>
    public Guid Correlation { get; set; }

    /// <summary>
    /// Property : Received
    /// </summary>
    public DateTime Received { get; set; }

    /// <summary>
    /// Property : ServiceName
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// Property : Type
    /// </summary>
    public SeverityType Type { get; set; }

    /// <summary>
    /// Property : Message
    /// </summary>
    public string Message { get; set; } 
}