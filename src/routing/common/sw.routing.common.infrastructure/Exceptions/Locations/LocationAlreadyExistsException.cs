using System;

namespace sw.routing.common.infrastructure.Exceptions.Locations
{
    public class LocationAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public LocationAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" Location with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}