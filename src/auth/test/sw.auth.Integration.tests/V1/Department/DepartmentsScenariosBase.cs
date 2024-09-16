using dottrack.auth.Integration.tests.Base;

namespace dottrack.auth.Integration.tests.V1.Department {
    public class DepartmentsScenariosBase : RegisterUserScenariosBase {
        private const string DepartmentApiUrlBase = "/api/v1/Departments";

        public static class Get {
            public static string GetDepartmentByIdAsync(long id) {
                return $"{DepartmentApiUrlBase}/{id}";
            }

            public static string GetDepartmentsAsync() {
                return $"{DepartmentApiUrlBase}";
            }
        }

        public static class Post {
            public static string PostDepartmentAsync() {
                return $"{DepartmentApiUrlBase}";
            }
        }

        public static class Put {
            public static string UpdateDepartmentAsync(long id) {
                return $"{DepartmentApiUrlBase}/{id}";
            }
        }
        public static class Delete {
            public static string DeleteSoftDepartmentAsync(long id) {
                return $"{DepartmentApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardDepartmentAsync(long id) {
                return $"{DepartmentApiUrlBase}/hard/{id}";
            }
        }
    }
}
