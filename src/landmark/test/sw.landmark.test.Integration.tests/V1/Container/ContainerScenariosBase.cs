using System;
using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.test.Integration.tests.V1.Container
{
    public class ContainerScenariosBase : RegisterUserScenariosBase
    {
        private const string ContainerApiUrlBase = "/api/v1/Containers";

        public static class Get
        {
            public static string GetContainerByIdAsync(long id)
            {
                return $"{ContainerApiUrlBase}/{id}";
            }

            public static string GetContainersAsync()
            {
                return $"{ContainerApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostContainerAsync()
            {
                return $"{ContainerApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateContainerAsync(long id)
            {
                return $"{ContainerApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftContainerAsync(long id)
            {
                return $"{ContainerApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardContainerAsync(long id)
            {
                return $"{ContainerApiUrlBase}/hard/{id}";
            }
        }
    }
}
