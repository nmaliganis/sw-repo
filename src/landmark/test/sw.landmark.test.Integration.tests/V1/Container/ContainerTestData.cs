using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Containers;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using System;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.asset.test.Integration.tests.V1.Container
{
    internal class CreateContainerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateContainerResourceParameters
                {
                    Name = "Cont1",
                    CodeErp = "10000",
                    Image = "img",
                    Description = "descr",
                    IsVisible = true,
                    Level = 1,
                    Latitude = 42.222,
                    Longitude = 22.222,
                    TimeToFull = 0,
                    LastServicedDate = DateTimeOffset.Now,
                    Status = 1,
                    MandatoryPickupDate = DateTimeOffset.Now,
                    MandatoryPickupActive = true,
                    Capacity = 1,
                    WasteType = 1,
                    Material = 1
                },
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100",
                    CodeErp = "100",
                    Params = "{}"
                },
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

    internal class UpdateContainerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateContainerResourceParameters
                {
                    Name = "Cont1",
                    CodeErp = "10000",
                    Image = "img",
                    Description = "descr",
                    IsVisible = true,
                    Level = 1,
                    Latitude = 42.222,
                    Longitude = 22.222,
                    TimeToFull = 0,
                    LastServicedDate = DateTimeOffset.Now,
                    Status = 1,
                    MandatoryPickupDate = DateTimeOffset.Now,
                    MandatoryPickupActive = true,
                    Capacity = 1,
                    WasteType = 1,
                    Material = 1
                },
                new UpdateContainerResourceParameters
                {
                    Name = "Cont2",
                    CodeErp = "20000",
                    Image = "img2",
                    Description = "descr2",
                    IsVisible = false,
                    Level = 2,
                    Latitude = 43.222,
                    Longitude = 23.222,
                    TimeToFull = 2,
                    LastServicedDate = DateTimeOffset.Now,
                    Status = 2,
                    MandatoryPickupDate = DateTimeOffset.Now,
                    MandatoryPickupActive = false,
                    Capacity = 2,
                    WasteType = 2,
                    Material = 2
                },
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100",
                    CodeErp = "100",
                    Params = "{}"
                },
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
}
