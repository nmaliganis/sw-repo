using System;

namespace sw.routing.common.infrastructure.Exceptions.Drivers
{
    public class DriverAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public DriverAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" Driver with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}