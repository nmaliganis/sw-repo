using System;

namespace sw.routing.common.infrastructure.Exceptions.Itineraries;

public class ItineraryAlreadyExistsException : Exception
{
    public string Name { get; }
    public string BrokenRules { get; }

    public ItineraryAlreadyExistsException(string name, string brokenRules)
    {
        Name = name;
        BrokenRules = brokenRules;
    }

    public override string Message => $" Itinerary with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
}