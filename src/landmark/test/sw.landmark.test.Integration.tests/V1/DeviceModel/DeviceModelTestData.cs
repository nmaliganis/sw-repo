using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.asset.test.Integration.tests.V1.DeviceModel
{
    internal class CreateDeviceModelTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateDeviceModelResourceParameters
                {
                    Name = "DM-1000",
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
            yield return new object[] {
                new CreateDeviceModelResourceParameters
                {
                    Name = "DM-1000",
                    CodeErp = "1000",
                    CodeName = "DM_1000",
                    Enabled = true
                },
                new UpdateDeviceModelResourceParameters
                {
                    Name = "DM-2000",
                    CodeErp = "2000",
                    CodeName = "DM_2000",
                    Enabled = false
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
