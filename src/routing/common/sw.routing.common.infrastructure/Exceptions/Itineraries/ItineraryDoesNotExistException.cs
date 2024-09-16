using System;

namespace sw.routing.common.infrastructure.Exceptions.Itineraries
{
    public class ItineraryDoesNotExistException : Exception
    {
        public long ItineraryId { get; }

        public ItineraryDoesNotExistException(long itineraryId)
        {
            this.ItineraryId = itineraryId;
        }

        public override string Message => $"Itinerary with Id: {ItineraryId}  doesn't exists!";
    }
}