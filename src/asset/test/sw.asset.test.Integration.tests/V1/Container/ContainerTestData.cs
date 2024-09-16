using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Containers;
using dottrack.asset.common.dtos.ResourceParameters.Companies;

namespace dottrack.asset.Integration.tests.V1.Container
{
    internal class CreateContainerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateContainerResourceParameters
                {
                    Name = "Cont1" + rnd.Next(),
                    CodeErp = "10000",
                    Image = "img",
                    Description = "descr",
                    IsVisible = true,
                    Level = 1,
                    Latitude = 42.222,
                    Longitude = 22.222,
                    TimeToFull = 0,
                    LastServicedDate = DateTime.Now,
                    Status = 1,
                    MandatoryPickupDate = DateTime.Now,
                    MandatoryPickupActive = true,
                    Capacity = 1,
                    WasteType = 1,
                    Material = 1
                },
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100" + rnd.Next(),
                    CodeErp = "100",
                    Params = "{}"
                },
                new CreateCompanyResourceParameters
                {
                    Name = "Com100" + rnd.Next(),
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
            Random rnd = new Random();
            yield return new object[] {
                new CreateContainerResourceParameters
                {
                    Name = "Cont1"+rnd.Next(),
                    CodeErp = "10000",
                    Image = "img",
                    Description = "descr",
                    IsVisible = true,
                    Level = 1,
                    Latitude = 42.222,
                    Longitude = 22.222,
                    TimeToFull = 0,
                    LastServicedDate = DateTime.Now,
                    Status = 1,
                    MandatoryPickupDate = DateTime.Now,
                    MandatoryPickupActive = true,
                    Capacity = 1,
                    WasteType = 1,
                    Material = 1
                },
                new UpdateContainerResourceParameters
                {
                    Name = "Cont2"+rnd.Next(),
                    CodeErp = "20000",
                    Image = "img2",
                    Description = "descr2",
                    IsVisible = false,
                    Level = 2,
                    Latitude = 43.222,
                    Longitude = 23.222,
                    TimeToFull = 2,
                    LastServicedDate = DateTime.Now,
                    Status = 2,
                    MandatoryPickupDate = DateTime.Now,
                    MandatoryPickupActive = false,
                    Capacity = 2,
                    WasteType = 2,
                    Material = 2
                },
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100" + rnd.Next(),
                    CodeErp = "100",
                    Params = "{}"
                },
                new CreateCompanyResourceParameters
                {
                    Name = "Com100" + rnd.Next(),
                    CodeErp = "100",
                    Description = "descr"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
