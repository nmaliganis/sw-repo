using System;

namespace sw.routing.common.infrastructure.Exceptions.Vehicles
{
    public class VehicleAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public VehicleAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" Vehicle with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}