using System;

namespace sw.asset.common.infrastructure.Exceptions.Sensors;

public class SensorAlreadyExistsException : Exception
{
    public string Imei { get; }
    public string BrokenRules { get; }

    public SensorAlreadyExistsException(string imei, string brokenRules)
    {
        Imei = imei;
        BrokenRules = brokenRules;
    }

    public override string Message => $" Sensor with Imei:{Imei} already Exists!\n Additional info:{BrokenRules}";
}//Class : SensorAlreadyExistsException