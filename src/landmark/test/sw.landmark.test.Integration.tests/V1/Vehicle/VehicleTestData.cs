using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Vehicles;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using System;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.asset.test.Integration.tests.V1.Vehicle
{
    internal class CreateVehicleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateVehicleResourceParameters
                {
                    Name = "Veh1",
                    CodeErp = "1000",
                    Image = "img",
                    Description = "descr",
                    NumPlate = "ABC-123",
                    Brand = "qwerty",
                    RegisteredDate = DateTimeOffset.Now,
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

    internal class UpdateVehicleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateVehicleResourceParameters
                {
                    Name = "Veh1",
                    CodeErp = "1000",
                    Image = "img",
                    Description = "descr",
                    NumPlate = "ABC-123",
                    Brand = "qwerty",
                    RegisteredDate = DateTimeOffset.Now,
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
                    Name = "Veh2",
                    CodeErp = "2000",
                    Image = "img2",
                    Description = "descr2",
                    NumPlate = "ABC-123-2",
                    Brand = "qwerty2",
                    RegisteredDate = DateTimeOffset.Now,
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
