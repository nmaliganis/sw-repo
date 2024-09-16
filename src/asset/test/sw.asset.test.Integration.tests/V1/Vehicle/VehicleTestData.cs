using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Vehicles;
using dottrack.asset.common.dtos.ResourceParameters.Companies;

namespace dottrack.asset.Integration.tests.V1.Vehicle
{
    internal class CreateVehicleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateVehicleResourceParameters
                {
                    Name = "Veh1"+rnd.Next(),
                    CodeErp = "1000",
                    Image = "img",
                    Description = "descr",
                    NumPlate = "ABC-"+rnd.Next(100,999),
                    Brand = "qwerty",
                    RegisteredDate = DateTime.Now,
                    Type = 1,
                    Status = 1,
                    Gas = 1,
                    Height = 1,
                    Width = 1,
                    Axels = 1,
                    MinTurnRadius = 1,
                    Length = 1
                },
                new CreateAssetCategoryResourceParameters
                {
                    Name = "AssCat100"+rnd.Next(),
                    CodeErp = "100",
                    Params = "{}"
                },
                new CreateCompanyResourceParameters
                {
                    Name = "Com100"+rnd.Next(),
                    CodeErp = "100",
                    Description = "descr"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateVehicleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateVehicleResourceParameters
                {
                    Name = "Veh1" + rnd.Next(),
                    CodeErp = "1000",
                    Image = "img",
                    Description = "descr",
                    NumPlate = "ABC-" + rnd.Next(100,999),
                    Brand = "qwerty",
                    RegisteredDate = DateTime.Now,
                    Type = 1,
                    Status = 1,
                    Gas = 1,
                    Height = 1,
                    Width = 1,
                    Axels = 1,
                    MinTurnRadius = 1,
                    Length = 1
                },
                new UpdateVehicleResourceParameters
                {
                    Name = "Veh2" + rnd.Next(),
                    CodeErp = "2000",
                    Image = "img2",
                    Description = "descr2",
                    NumPlate = "ABC-" + rnd.Next(100,999),
                    Brand = "qwerty2",
                    RegisteredDate = DateTime.Now,
                    Type = 2,
                    Status = 2,
                    Gas = 2,
                    Height = 2,
                    Width = 2,
                    Axels = 2,
                    MinTurnRadius = 2,
                    Length = 2
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
