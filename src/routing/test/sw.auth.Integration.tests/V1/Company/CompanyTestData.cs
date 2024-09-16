using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.auth.common.dtos.ResourceParameters.Companies;

namespace dottrack.auth.Integration.tests.V1.Company {
    internal class CreateCompanyTestData : IEnumerable<object[]> {
        public IEnumerator<object[]> GetEnumerator() {
            Random rnd = new Random();  
            yield return new object[] {
                new CreateCompanyResourceParameters
                {
                    Name = "TestComp"+rnd.Next(),
                    CodeErp = "100",
                    Description = "descr"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }

    internal class UpdateCompanyTestData : IEnumerable<object[]> {
        public IEnumerator<object[]> GetEnumerator() {
            Random rnd = new Random();
            yield return new object[] {
                new CreateCompanyResourceParameters
                {
                    Name = "TestComp"+rnd.Next(),
                    CodeErp = "100",
                    Description = "Company description"
                },
                new UpdateCompanyResourceParameters
                {
                    Name = "TestComp"+rnd.Next(),
                    CodeErp = "200",
                    Description = "Updated description"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
