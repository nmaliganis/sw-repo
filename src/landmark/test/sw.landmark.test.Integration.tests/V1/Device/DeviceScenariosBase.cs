using System;
using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.test.Integration.tests.V1.Device
{
    public class DeviceScenariosBase : RegisterUserScenariosBase
    {
        private const string DeviceApiUrlBase = "/api/v1/Devices";

        public static class Get
        {
            public static string GetDeviceByIdAsync(long id)
            {
                return $"{DeviceApiUrlBase}/{id}";
            }

            public static string GetDevicesAsync()
            {
                return $"{DeviceApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostDeviceAsync()
            {
                return $"{DeviceApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateDeviceAsync(long id)
            {
                return $"{DeviceApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftDeviceAsync(long id)
            {
                return $"{DeviceApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardDeviceAsync(long id)
            {
                return $"{DeviceApiUrlBase}/hard/{id}";
            }
        }
    }
}
