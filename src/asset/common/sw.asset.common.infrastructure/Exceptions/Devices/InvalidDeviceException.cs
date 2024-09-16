using System;

namespace sw.asset.common.infrastructure.Exceptions.Devices;

public class InvalidDeviceException : Exception
{
  public string BrokenRules { get; private set; }

  public InvalidDeviceException(string brokenRules)
  {
    BrokenRules = brokenRules;
  }
}//Class : InvalidDeviceException