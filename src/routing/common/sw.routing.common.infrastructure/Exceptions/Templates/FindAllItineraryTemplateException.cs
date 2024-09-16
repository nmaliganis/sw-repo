using System;

namespace sw.routing.common.infrastructure.Exceptions.Templates;

public class FindAllItineraryTemplateException : Exception
{
    private readonly string _messageDetails;

    public FindAllItineraryTemplateException(string messageDetails)
    {
        this._messageDetails = messageDetails;
    }

    public override string Message => $"Find all Itinerary Template error: {_messageDetails}";
}