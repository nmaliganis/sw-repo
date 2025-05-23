﻿using dottrack.auth.Integration.tests.Base;

namespace dottrack.auth.Integration.tests.V1.Company {
    public class CompanyScenariosBase : RegisterUserScenariosBase {
        private const string CompanyApiUrlBase = "/api/v1/companies";

        public static class Get {
            public static string GetCompanyByIdAsync(long id) {
                return $"{CompanyApiUrlBase}/{id}";
            }

            public static string GetCompaniesAsync() {
                return $"{CompanyApiUrlBase}";
            }
        }

        public static class Post {
            public static string PostCompanyAsync() {
                return $"{CompanyApiUrlBase}";
            }
        }

        public static class Put {
            public static string UpdateCompanyAsync(long id) {
                return $"{CompanyApiUrlBase}/{id}";
            }
        }
        public static class Delete {
            public static string DeleteSoftCompanyAsync(long id) {
                return $"{CompanyApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardCompanyAsync(long id) {
                return $"{CompanyApiUrlBase}/hard/{id}";
            }
        }
    }
}
