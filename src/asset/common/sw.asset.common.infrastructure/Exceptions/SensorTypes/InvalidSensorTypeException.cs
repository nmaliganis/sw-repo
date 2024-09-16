using System;

namespace sw.asset.common.infrastructure.Exceptions.SensorTypes;

public class InvalidSensorTypeException : Exception
{
  public string BrokenRules { get; private set; }

  public InvalidSensorTypeException(string brokenRules)
  {
    BrokenRules = brokenRules;
  }
}//Class : InvalidSensorTypeException