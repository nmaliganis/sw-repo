using System;

namespace sw.asset.common.infrastructure.Exceptions.Sensors;

public class SensorDoesNotExistException : Exception
{
    public long SensorId { get; }
    public string DeviceImei { get; }

    public SensorDoesNotExistException(long sensorId)
    {
        this.SensorId = sensorId;
    }

    public SensorDoesNotExistException(string deviceImei)
    {
        this.DeviceImei = deviceImei;
    }

    public override string Message => $"Sensor with Id: {this.SensorId} or DeviceImei: {this.DeviceImei} doesn't exists!";
}//Class : SensorDoesNotExistException