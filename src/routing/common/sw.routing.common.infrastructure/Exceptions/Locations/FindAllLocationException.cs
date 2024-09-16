using System;

namespace sw.routing.common.infrastructure.Exceptions.Locations;

public class FindAllLocationException : Exception
{
    private readonly string _messageDetails;

    public FindAllLocationException(string messageDetails)
    {
        this._messageDetails = messageDetails;
    }

    public override string Message => $"Find all Itinerary Template error: {_messageDetails}";
}