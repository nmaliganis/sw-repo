using System;

namespace sw.routing.common.infrastructure.Exceptions;

/// <summary>
/// Exception type for domain exceptions
/// </summary>
public class RoutingDomainException : Exception
{
    public RoutingDomainException() { }

    public RoutingDomainException(string message)
      : base(message) { }

    public RoutingDomainException(string message, Exception innerException)
      : base(message, innerException) { }
}//Class : RoutingDomainException