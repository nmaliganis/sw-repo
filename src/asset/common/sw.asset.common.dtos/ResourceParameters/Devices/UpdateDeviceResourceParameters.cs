using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Devices
{
    public class UpdateDeviceResourceParameters
    {
        [Required]
        public long DeviceModelId { get; set; }

        [Required]
        [StringLength(250)]
        public string Imei { get; set; }

        [Required]
        [StringLength(250)]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string ActivationCode { get; set; }

        [Required]
        public DateTime ActivationDate { get; set; }

        [Required]
        public long ActivationBy { get; set; }

        [Required]
        [StringLength(10)]
        public string ProvisioningCode { get; set; }

        [Required]
        public long ProvisioningBy { get; set; }

        [Required]
        public DateTime ProvisioningDate { get; set; }

        [Required]
        [StringLength(10)]
        public string ResetCode { get; set; }

        [Required]
        public long ResetBy { get; set; }

        [Required]
        public DateTime ResetDate { get; set; }

        [Required]
        public bool Activated { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public string IpAddress { get; set; }

        [Required]
        public DateTime LastRecordedDate { get; set; }

        [Required]
        public DateTime LastReceivedDate { get; set; }

        [Required]
        public string CodeErp { get; set; }
    }
}
