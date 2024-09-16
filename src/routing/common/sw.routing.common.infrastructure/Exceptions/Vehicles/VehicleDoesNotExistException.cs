using System;

namespace sw.routing.common.infrastructure.Exceptions.Vehicles
{
    public class VehicleDoesNotExistException : Exception
    {
        public long VehicleId { get; }

        public VehicleDoesNotExistException(long vehicleId)
        {
            this.VehicleId = vehicleId;
        }

        public override string Message => $"Vehicle with Id: {VehicleId}  doesn't exists!";
    }
}