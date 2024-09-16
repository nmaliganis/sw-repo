using System;

namespace sw.routing.common.infrastructure.Exceptions.Itineraries
{
    public class ItineraryDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public ItineraryDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }

        public override string Message => $" Itinerary with Name: {Name} was not made Persistent!";
    }
}