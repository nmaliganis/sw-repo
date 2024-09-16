using dottrack.common.dtos.ResourceParameters.Roles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dottrack.auth.Integration.tests.V1.Roles
{
    internal class CreateRoleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Random rnd = new Random();
            yield return new object[] {
                new CreateRoleResourceParameters
                {
                    Name = "TestRole"+rnd.Next(),
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    internal class UpdateRoleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Random rnd = new Random();
            yield return new object[] {
                new CreateRoleResourceParameters
                {
                    Name = "TestRole"+rnd.Next(),
                },
                new UpdateRoleResourceParameters
                {
                    Name = "UpdatedTestRole"+rnd.Next(),

                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

