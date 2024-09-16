using System;

namespace sw.asset.common.infrastructure.Exceptions.Devices;

public class DeviceAlreadyExistsException : Exception {
  public string Imei { get; }
  public string BrokenRules { get; }

  public DeviceAlreadyExistsException(string imei, string brokenRules) {
    this.Imei = imei;
    this.BrokenRules = brokenRules;
  }

  public override string Message => $" Device with Imei:{this.Imei} already Exists!\n Additional info:{this.BrokenRules}";
}//Class : DeviceAlreadyExistsException