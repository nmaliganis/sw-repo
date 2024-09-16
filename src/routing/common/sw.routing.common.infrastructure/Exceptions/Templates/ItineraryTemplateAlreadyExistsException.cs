using System;

namespace sw.routing.common.infrastructure.Exceptions.Templates
{
    public class ItineraryTemplateAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public ItineraryTemplateAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" ItineraryTemplate with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}