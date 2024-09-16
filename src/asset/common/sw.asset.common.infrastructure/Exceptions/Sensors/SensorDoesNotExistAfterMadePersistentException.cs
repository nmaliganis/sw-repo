using System;

namespace sw.asset.common.infrastructure.Exceptions.Sensors;

public class SensorDoesNotExistAfterMadePersistentException : Exception
{
  public string Imei { get; private set; }

  public SensorDoesNotExistAfterMadePersistentException(string imei)
  {
    Imei = imei;
  }

  public override string Message => $" Sensor with Imei: {Imei} was not made Persistent!";
}// Class : SensorDoesNotExistAfterMadePersistentException