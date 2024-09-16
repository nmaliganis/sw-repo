using System;

namespace sw.onboarding.common.infrastructure.Exceptions;

/// <summary>
/// Exception type for domain exceptions
/// </summary>
public class AuthDomainException : Exception {
    public AuthDomainException() { }

    public AuthDomainException(string message)
        : base(message) { }

    public AuthDomainException(string message, Exception innerException)
        : base(message, innerException) { }

}//Class : AuthDomainException