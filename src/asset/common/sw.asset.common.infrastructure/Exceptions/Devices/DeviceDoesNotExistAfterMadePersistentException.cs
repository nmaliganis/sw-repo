using System;

namespace sw.asset.common.infrastructure.Exceptions.Devices;

public class DeviceDoesNotExistAfterMadePersistentException : Exception
{
  public string Imei { get; private set; }

  public DeviceDoesNotExistAfterMadePersistentException(string imei)
  {
    Imei = imei;
  }

  public override string Message => $" Device with Imei: {Imei} was not made Persistent!";
}// Class : DeviceDoesNotExistAfterMadePersistentException