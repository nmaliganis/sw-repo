using System;

namespace sw.asset.common.infrastructure.Exceptions.SensorTypes;

public class SensorTypeDoesNotExistException : Exception {
  public long SensorTypeId { get; }

  public SensorTypeDoesNotExistException(long sensorTypeId) {
    this.SensorTypeId = sensorTypeId;
  }

  public override string Message => $"SensorType with Id: {this.SensorTypeId}  doesn't exists!";
}//Class : SensorTypeDoesNotExistException
