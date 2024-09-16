using System;
using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.test.Integration.tests.V1.Vehicle
{
    public class VehicleScenariosBase : RegisterUserScenariosBase
    {
        private const string VehicleApiUrlBase = "/api/v1/Vehicles";

        public static class Get
        {
            public static string GetVehicleByIdAsync(long id)
            {
                return $"{VehicleApiUrlBase}/{id}";
            }

            public static string GetVehiclesAsync()
            {
                return $"{VehicleApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostVehicleAsync()
            {
                return $"{VehicleApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateVehicleAsync(long id)
            {
                return $"{VehicleApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftVehicleAsync(long id)
            {
                return $"{VehicleApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardVehicleAsync(long id)
            {
                return $"{VehicleApiUrlBase}/hard/{id}";
            }
        }
    }
}
