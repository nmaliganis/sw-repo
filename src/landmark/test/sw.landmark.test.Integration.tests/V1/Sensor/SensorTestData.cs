using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Containers;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.ResourceParameters.Devices;
using dottrack.asset.common.dtos.ResourceParameters.Sensors;
using dottrack.asset.common.dtos.ResourceParameters.SensorTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using dottrack.asset.common.dtos.ResourceParameters.Sensor;

namespace dottrack.asset.test.Integration.tests.V1.Sensor
{
    internal class CreateSensorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateSensorResourceParameters
                {
                    Params = "{}",
                    Name = "Sen1",
                    CodeErp = "1000",
                    IsActive = true,
                    IsVisible = true,
                    Order = 1,
                    MinValue = 1,
                    MaxValue = 1,
                    MinNotifyValue = 1,
                    MaxNotifyValue = 1,
                    LastValue = 1,
                    LastRecordedDate = DateTimeOffset.Now,
                    LastReceivedDate = DateTimeOffset.Now,
                    HighThreshold = 1,
                    LowThreshold = 1,
                    SamplingInterval = 1,
                    ReportingInterval = 1
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
                },
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
                new CreateDeviceModelResourceParameters
                {
                    Name = "DM-1000",
                    CodeErp = "1000",
                    CodeName = "DM_1000",
                    Enabled = true
                },
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
                new CreateSensorTypeResourceParameters
                {
                    Name = "ST-100",
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
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class UpdateSensorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new CreateSensorResourceParameters
                {
                    Params = "{}",
                    Name = "Sen1",
                    CodeErp = "1000",
                    IsActive = true,
                    IsVisible = true,
                    Order = 1,
                    MinValue = 1,
                    MaxValue = 1,
                    MinNotifyValue = 1,
                    MaxNotifyValue = 1,
                    LastValue = 1,
                    LastRecordedDate = DateTimeOffset.Now,
                    LastReceivedDate = DateTimeOffset.Now,
                    HighThreshold = 1,
                    LowThreshold = 1,
                    SamplingInterval = 1,
                    ReportingInterval = 1
                },
                new UpdateSensorResourceParameters
                {
                    Params = "{\"obj\": \"Object\"}",
                    Name = "Sen2",
                    CodeErp = "2000",
                    IsActive = false,
                    IsVisible = false,
                    Order = 2,
                    MinValue = 2,
                    MaxValue = 2,
                    MinNotifyValue = 2,
                    MaxNotifyValue = 2,
                    LastValue = 2,
                    LastRecordedDate = DateTimeOffset.Now,
                    LastReceivedDate = DateTimeOffset.Now,
                    HighThreshold = 2,
                    LowThreshold = 2,
                    SamplingInterval = 2,
                    ReportingInterval = 2
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
                },
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
                new CreateDeviceModelResourceParameters
                {
                    Name = "DM-1000",
                    CodeErp = "1000",
                    CodeName = "DM_1000",
                    Enabled = true
                },
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
                new CreateSensorTypeResourceParameters
                {
                    Name = "ST-100",
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
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
