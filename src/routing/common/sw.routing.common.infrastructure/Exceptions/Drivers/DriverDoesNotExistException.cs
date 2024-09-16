using System;

namespace sw.routing.common.infrastructure.Exceptions.Drivers
{
    public class DriverDoesNotExistException : Exception
    {
        public long DriverId { get; }

        public DriverDoesNotExistException(long driverId)
        {
            this.DriverId = driverId;
        }

        public override string Message => $"Driver with Id: {DriverId}  doesn't exists!";
    }
}