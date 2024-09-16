using System;

namespace sw.routing.common.infrastructure.Exceptions.Vehicles
{
    public class InvalidVehicleException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidVehicleException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}