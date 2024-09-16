using System;

namespace sw.asset.common.infrastructure.Exceptions.SensorTypes;

public class SensorTypeDoesNotExistAfterMadePersistentException : Exception
{
  public string Name { get; private set; }

  public SensorTypeDoesNotExistAfterMadePersistentException(string name)
  {
    Name = name;
  }

  public override string Message => $" SensorType with Name: {Name} was not made Persistent!";
}// Class : SensorTypeDoesNotExistAfterMadePersistentException