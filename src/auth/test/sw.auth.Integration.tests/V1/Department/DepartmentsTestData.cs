using dottrack.common.dtos.ResourceParameters.Departments;
using System;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.auth.Integration.tests.V1.Department{
    internal class CreateDepartmentTestData : IEnumerable<object[]> {
        public IEnumerator<object[]> GetEnumerator() {
            Random rnd = new Random(); 
            yield return new object[] {
                
                new CreateDepartmentResourceParameters
                {

                    Name = "TestComp"+rnd.Next(),
                    CodeErp = "83",
                    CompanyId = 1
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }

    internal class UpdateDepartmentTestData : IEnumerable<object[]> {
        public IEnumerator<object[]> GetEnumerator() {
            Random rnd = new Random();
            yield return new object[] {
                new CreateDepartmentResourceParameters
                {
                    Name = "TestComp"+rnd.Next(),
                    CodeErp = "100",
                    CompanyId = 1,
                    Notes = "Test Department"
                },
                new UpdateDepartmentResourceParameters
                {
                    Name = "TestComp"+rnd.Next(),
                    CodeErp = "200",
                    //CompanyId = 1,
                    Notes = "Test Department Updated"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
