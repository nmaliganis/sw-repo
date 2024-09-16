using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using Microsoft.Azure.Amqp.Framing;

namespace dottrack.asset.Integration.tests.V1.AssetCategory
{
    internal class CreateAssetCategoryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100"+rnd.Next(),
                    CodeErp = "100",
                    Params = "{Params:delegate}"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateAssetCategoryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100"+rnd.Next(),
                    CodeErp = "100",
                    Params = "{Params:delegate}"
                },
                new UpdateAssetCategoryResourceParameters
                {
                    Name = "AssCat200"+rnd.Next(),
                    CodeErp = "200",
                    Params = "{\"obj\": \"Object\"}"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
