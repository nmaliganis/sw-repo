using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.asset.test.Integration.tests.V1.AssetCategory
{
    internal class CreateAssetCategoryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100",
                    CodeErp = "100",
                    Params = "{}"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateAssetCategoryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100",
                    CodeErp = "100",
                    Params = "{}"
                },
                new UpdateAssetCategoryResourceParameters
                {
                    Name = "AssCat200",
                    CodeErp = "200",
                    Params = "{\"obj\": \"Object\"}"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
