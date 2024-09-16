using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.asset.common.dtos.ResourceParameters.SensorTypes;

namespace dottrack.asset.Integration.tests.V1.SensorType
{
    internal class CreateSensorTypeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateSensorTypeResourceParameters
                {
                    Name = "ST-100" + rnd.Next(),
                    ShowAtStatus = true,
                    StatusExpiryMinutes = 1000,
                    ShowOnMap = true,
                    ShowAtReport = true,
                    ShowAtChart = true,
                    ResetValues = true,
                    SumValues = true,
                    Precision = 1,
                    Tunit = "Newton (N)",
                    CalcPosition = true,
                    CodeErp = "100",
                    
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateSensorTypeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateSensorTypeResourceParameters
                {
                    Name = "ST-100" + rnd.Next(),
                    ShowAtStatus = true,
                    StatusExpiryMinutes = 1000,
                    ShowOnMap = true,
                    ShowAtReport = true,
                    ShowAtChart = true,
                    ResetValues = true,
                    SumValues = true,
                    Precision = 1,
                    Tunit = "Newton (N)",
                    CalcPosition = true,
                    CodeErp = "100"
                },
                new UpdateSensorTypeResourceParameters
                {
                    Name = "ST-200" + rnd.Next(),
                    ShowAtStatus = false,
                    StatusExpiryMinutes = 2000,
                    ShowOnMap = false,
                    ShowAtReport = false,
                    ShowAtChart = false,
                    ResetValues = false,
                    SumValues = false,
                    Precision = 2,
                    Tunit = "Dyne (dyn)",
                    CalcPosition = false,
                    CodeErp = "200"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
