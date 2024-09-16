using System;
using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.test.Integration.tests.V1.SensorType
{
    public class SensorTypeScenariosBase : RegisterUserScenariosBase
    {
        private const string SensorTypeApiUrlBase = "/api/v1/SensorTypes";

        public static class Get
        {
            public static string GetSensorTypeByIdAsync(long id)
            {
                return $"{SensorTypeApiUrlBase}/{id}";
            }

            public static string GetSensorTypesAsync()
            {
                return $"{SensorTypeApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostSensorTypeAsync()
            {
                return $"{SensorTypeApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateSensorTypeAsync(long id)
            {
                return $"{SensorTypeApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftSensorTypeAsync(long id)
            {
                return $"{SensorTypeApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardSensorTypeAsync(long id)
            {
                return $"{SensorTypeApiUrlBase}/hard/{id}";
            }
        }
    }
}
