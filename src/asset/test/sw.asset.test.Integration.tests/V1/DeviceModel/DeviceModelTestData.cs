using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;

namespace dottrack.asset.Integration.tests.V1.DeviceModel
{
    internal class CreateDeviceModelTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateDeviceModelResourceParameters
                {
                    Name = "DM-1000" + rnd.Next(),
                    CodeErp = "1000",
                    CodeName = "DM_1000",
                    Enabled = true
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateDeviceModelTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateDeviceModelResourceParameters
                {
                    Name = "DM-1000" + rnd.Next(),
                    CodeErp = "1000",
                    CodeName = "DM_1000",
                    Enabled = true
                },
                new UpdateDeviceModelResourceParameters
                {
                    Name = "DM-2000" + rnd.Next(),
                    CodeErp = "2000",
                    CodeName = "DM_2000",
                    Enabled = false
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
