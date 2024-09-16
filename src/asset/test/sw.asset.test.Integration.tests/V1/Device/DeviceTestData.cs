using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.ResourceParameters.Devices;

namespace dottrack.asset.Integration.tests.V1.Device
{
    internal class CreateDeviceTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rnd = new Random();
            yield return new object[] {
                new CreateDeviceResourceParameters
                {
                    Imei = "100"+ rnd.Next(),
                    SerialNumber = "sn_100"+ rnd.Next(),
                    PhoneNumber = "6969696969",
                    IpAddress = rnd.Next(10,255)+".168."+rnd.Next(10,255)+".1",
                },
                new CreateDeviceModelResourceParameters
                {
                    Name = "DM-1000"+rnd.Next(),
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
            var rnd = new Random();
            yield return new object[] {
                new CreateDeviceResourceParameters
                 {
                    Imei = "100" + rnd.Next(),
                    SerialNumber = "sn_100"+ rnd.Next(),
                    PhoneNumber = "6969696969",
                    IpAddress = rnd.Next(10,255)+".168."+rnd.Next(10,255)+".1"
                },
                new UpdateDeviceResourceParameters
                 {
                    Imei = "200" + rnd.Next(),
                    SerialNumber = "sn_200"+rnd.Next(),
                    ActivationCode = "activation",
                    ActivationDate = DateTime.Now,
                    ActivationBy = 2,
                    ProvisioningCode = "provision",
                    ProvisioningBy = 2,
                    ProvisioningDate = DateTime.Now,
                    ResetCode = "reset",
                    ResetBy = 2,
                    ResetDate = DateTime.Now,
                    Activated = false,
                    Enabled = false,
                    IpAddress = "192.168.2.2",
                    LastRecordedDate = DateTime.Now,
                    LastReceivedDate = DateTime.Now,
                    CodeErp = "200"
                },
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
}
