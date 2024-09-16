using System;

namespace sw.asset.common.infrastructure.Exceptions.SensorTypes;

public class SensorTypeAlreadyExistsException : Exception {
  public string Name { get; }
  public string BrokenRules { get; }

  public SensorTypeAlreadyExistsException(string name, string brokenRules) {
    this.Name = name;
    this.BrokenRules = brokenRules;
  }

  public override string Message => $" SensorType with Name:{this.Name} already Exists!\n Additional info:{this.BrokenRules}";
}//Class : SensorTypeAlreadyExistsException
