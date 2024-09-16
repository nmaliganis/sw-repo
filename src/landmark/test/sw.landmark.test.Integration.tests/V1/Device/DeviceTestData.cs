using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.ResourceParameters.Devices;
using System;
using System.Collections;
using System.Collections.Generic;

namespace dottrack.asset.test.Integration.tests.V1.Device
{
    internal class CreateDeviceTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateDeviceResourceParameters
                {
                    Imei = "100",
                    SerialNumber = "sn_100",
                    ActivationCode = "activation",
                    ActivationDate = DateTimeOffset.Now,
                    ActivationBy = 1,
                    ProvisioningCode = "provision",
                    ProvisioningBy = 1,
                    ProvisioningDate = DateTimeOffset.Now,
                    ResetCode = "reset",
                    ResetBy = 1,
                    ResetDate = DateTimeOffset.Now,
                    Activated = true,
                    Enabled = true,
                    IpAddress = "192.168.1.1",
                    LastRecordedDate = DateTimeOffset.Now,
                    LastReceivedDate = DateTimeOffset.Now,
                    CodeErp = "100"
                },
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

    internal class UpdateDeviceTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateDeviceResourceParameters
                 {
                    Imei = "100",
                    SerialNumber = "sn_100",
                    ActivationCode = "activation",
                    ActivationDate = DateTimeOffset.Now,
                    ActivationBy = 1,
                    ProvisioningCode = "provision",
                    ProvisioningBy = 1,
                    ProvisioningDate = DateTimeOffset.Now,
                    ResetCode = "reset",
                    ResetBy = 1,
                    ResetDate = DateTimeOffset.Now,
                    Activated = true,
                    Enabled = true,
                    IpAddress = "192.168.1.1",
                    LastRecordedDate = DateTimeOffset.Now,
                    LastReceivedDate = DateTimeOffset.Now,
                    CodeErp = "100"
                },
                new UpdateDeviceResourceParameters
                 {
                    Imei = "200",
                    SerialNumber = "sn_200",
                    ActivationCode = "activation",
                    ActivationDate = DateTimeOffset.Now,
                    ActivationBy = 2,
                    ProvisioningCode = "provision",
                    ProvisioningBy = 2,
                    ProvisioningDate = DateTimeOffset.Now,
                    ResetCode = "reset",
                    ResetBy = 2,
                    ResetDate = DateTimeOffset.Now,
                    Activated = false,
                    Enabled = false,
                    IpAddress = "192.168.2.2",
                    LastRecordedDate = DateTimeOffset.Now,
                    LastReceivedDate = DateTimeOffset.Now,
                    CodeErp = "200"
                },
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
}
