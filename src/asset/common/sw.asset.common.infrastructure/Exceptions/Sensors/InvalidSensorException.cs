using System;

namespace sw.asset.common.infrastructure.Exceptions.Sensors;

public class InvalidSensorException : Exception
{
  public string BrokenRules { get; private set; }

  public InvalidSensorException(string brokenRules)
  {
    BrokenRules = brokenRules;
  }
}//Class : InvalidSensorException