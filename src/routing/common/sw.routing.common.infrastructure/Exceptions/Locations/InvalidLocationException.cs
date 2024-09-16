using System;

namespace sw.routing.common.infrastructure.Exceptions.Locations
{
    public class InvalidLocationException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidLocationException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}