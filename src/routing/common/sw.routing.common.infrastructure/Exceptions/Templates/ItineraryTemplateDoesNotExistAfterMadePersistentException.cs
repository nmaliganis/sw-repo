using System;

namespace sw.routing.common.infrastructure.Exceptions.Templates
{
    public class ItineraryTemplateDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public ItineraryTemplateDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }

        public override string Message => $" ItineraryTemplate with Name: {Name} was not made Persistent!";
    }
}