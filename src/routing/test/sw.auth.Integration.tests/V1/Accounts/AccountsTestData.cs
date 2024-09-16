using dottrack.common.dtos.Vms.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
namespace dottrack.auth.Integration.tests.V1.Accounts
{
    internal class CreateAccountsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new UserForRegistrationUiModel
                {
                    Login = "test@test.gr",
                    Password = "testpassword",
                    Email = "test@test.gr",
                    Company = 1,
                    Firstname = "Test",
                    Lastname = "Test",
                    Gender = 1,
                    AddressCity = "Test",
                    AddressPostCode = "Test",
                    AddressRegion = "Test",
                    DepartmentIds = new List<long>{1},
                    ExtMobile = "Test", 
                    ExtPhone = "Test",
                    GenderValue = "male",
                    Mobile  = "Test",
                    Phone = "Test",
                    Street = "Test",
                    RoleIds = new List<long>{3},
                    StreetNumber = "Test22",
                    Notes = "Test user account created"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}