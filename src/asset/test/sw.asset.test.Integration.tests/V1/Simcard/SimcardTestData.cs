using dottrack.asset.common.dtos.ResourceParameters.Simcards;
using System;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.asset.Integration.tests.V1.Simcard
{
    internal class CreateSimcardTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateSimcardResourceParameters
                {
                    Number = "69"+ rnd.Next(10000000,99999999),
                    CodeErp = "100"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateSimcardTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateSimcardResourceParameters
                {
                     Number = "69"+ rnd.Next(10000000,99999999),
                     CodeErp = "100",

                },
                new UpdateSimcardResourceParameters
                {
                    Number = "69" + rnd.Next(10000000, 99999999),
                    CodeErp = "100",
                    Notes= "Test note updated"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
