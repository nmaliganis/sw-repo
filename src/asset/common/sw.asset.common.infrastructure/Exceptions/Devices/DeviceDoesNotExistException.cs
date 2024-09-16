using System;

namespace sw.asset.common.infrastructure.Exceptions.Devices;

public class DeviceDoesNotExistException : Exception {
  public long DeviceId { get; }

  public DeviceDoesNotExistException(long deviceId) {
    this.DeviceId = deviceId;
  }

  public override string Message => $"Device with Id: {this.DeviceId}  doesn't exists!";
}//Class : DeviceDoesNotExistException