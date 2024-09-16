using dottrack.common.dtos.Vms.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.auth.common.dtos.Vms.Accounts;

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
                    Firstname = "Test",
                    Lastname = "Test",
                    Gender = 1,
                    AddressCity = "Test",
                    AddressPostCode = "Test",
                    AddressRegion = "Test",
                    ExtMobile = "Test", 
                    ExtPhone = "Test",
                    GenderValue = "male",
                    Mobile  = "Test",
                    Phone = "Test",
                    Street = "Test",
                    StreetNumber = "Test22",
                    Notes = "Test user account created"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}