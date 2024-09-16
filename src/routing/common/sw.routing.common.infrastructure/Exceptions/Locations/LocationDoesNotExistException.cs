using System;

namespace sw.routing.common.infrastructure.Exceptions.Locations
{
    public class LocationDoesNotExistException : Exception
    {
        public long LocationId { get; }

        public LocationDoesNotExistException(long locationId)
        {
            this.LocationId = locationId;
        }

        public override string Message => $"Location with Id: {LocationId}  doesn't exists!";
    }
}