using System;

namespace sw.routing.common.infrastructure.Exceptions.Locations
{
    public class LocationDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public LocationDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }

        public override string Message => $" Location with Name: {Name} was not made Persistent!";
    }
}