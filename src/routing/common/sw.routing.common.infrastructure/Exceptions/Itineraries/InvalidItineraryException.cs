using System;

namespace sw.routing.common.infrastructure.Exceptions.Itineraries;

public class InvalidItineraryException : Exception
{
    public string BrokenRules { get; private set; }

    public InvalidItineraryException(string brokenRules)
    {
        BrokenRules = brokenRules;
    }
}