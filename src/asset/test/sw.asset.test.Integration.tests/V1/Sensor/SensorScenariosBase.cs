using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.Integration.tests.V1.Sensor
{
    public class SensorScenariosBase : RegisterUserScenariosBase
    {
        private const string SensorApiUrlBase = "/api/v1/Sensors";

        public static class Get
        {
            public static string GetSensorByIdAsync(long id)
            {
                return $"{SensorApiUrlBase}/{id}";
            }

            public static string GetSensorsAsync()
            {
                return $"{SensorApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostSensorAsync()
            {
                return $"{SensorApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateSensorAsync(long id)
            {
                return $"{SensorApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftSensorAsync(long id)
            {
                return $"{SensorApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardSensorAsync(long id)
            {
                return $"{SensorApiUrlBase}/hard/{id}";
            }
        }
    }
}
