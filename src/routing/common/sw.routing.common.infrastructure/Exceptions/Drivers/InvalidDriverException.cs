using System;

namespace sw.routing.common.infrastructure.Exceptions.Drivers
{
    public class InvalidDriverException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidDriverException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}