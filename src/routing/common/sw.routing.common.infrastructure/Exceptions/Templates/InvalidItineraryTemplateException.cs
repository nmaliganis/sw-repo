using System;

namespace sw.routing.common.infrastructure.Exceptions.Templates
{
    public class InvalidItineraryTemplateException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidItineraryTemplateException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}