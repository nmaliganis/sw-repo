using System;

namespace sw.asset.common.infrastructure.Exceptions;

/// <summary>
/// Exception type for domain exceptions
/// </summary>
public class AssetDomainException : Exception {
  public AssetDomainException() { }

  public AssetDomainException(string message)
    : base(message) { }

  public AssetDomainException(string message, Exception innerException)
    : base(message, innerException) { }

}//Class : AssetDomainException