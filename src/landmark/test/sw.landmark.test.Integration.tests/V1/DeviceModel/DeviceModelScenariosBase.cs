using System;
using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.test.Integration.tests.V1.DeviceModel
{
    public class DeviceModelScenariosBase : RegisterUserScenariosBase
    {
        private const string DeviceModelApiUrlBase = "/api/v1/DeviceModels";

        public static class Get
        {
            public static string GetDeviceModelByIdAsync(long id)
            {
                return $"{DeviceModelApiUrlBase}/{id}";
            }

            public static string GetDeviceModelsAsync()
            {
                return $"{DeviceModelApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostDeviceModelAsync()
            {
                return $"{DeviceModelApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateDeviceModelAsync(long id)
            {
                return $"{DeviceModelApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftDeviceModelAsync(long id)
            {
                return $"{DeviceModelApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardDeviceModelAsync(long id)
            {
                return $"{DeviceModelApiUrlBase}/hard/{id}";
            }
        }
    }
}
