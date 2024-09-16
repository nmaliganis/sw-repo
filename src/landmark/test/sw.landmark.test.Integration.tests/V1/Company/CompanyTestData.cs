using dottrack.asset.common.dtos.ResourceParameters.Companies;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.asset.test.Integration.tests.V1.Company
{
    internal class CreateCompanyTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateCompanyResourceParameters
                {
                    Name = "Com100",
                    CodeErp = "100",
                    Description = "descr"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateCompanyTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateCompanyResourceParameters
                {
                    Name = "Com100",
                    CodeErp = "100",
                    Description = "descr"
                },
                new UpdateCompanyResourceParameters
                {
                    Name = "Com200",
                    CodeErp = "200",
                    Description = "descr"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
