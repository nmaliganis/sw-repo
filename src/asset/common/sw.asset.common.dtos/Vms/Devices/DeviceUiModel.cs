using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Devices
{
    public class DeviceUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required]
        [Editable(true)]
        public long DeviceModelId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Imei { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string SerialNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ActivationCode { get; set; }

        [Required]
        [Editable(true)]
        public DateTime ActivationDate { get; set; }

        [Required]
        [Editable(true)]
        public long ActivationBy { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ProvisioningCode { get; set; }

        [Required]
        [Editable(true)]
        public long ProvisioningBy { get; set; }

        [Required]
        [Editable(true)]
        public DateTime ProvisioningDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ResetCode { get; set; }

        [Required]
        [Editable(true)]
        public long ResetBy { get; set; }

        [Required]
        [Editable(true)]
        public DateTime ResetDate { get; set; }

        [Required]
        [Editable(true)]
        public bool Activated { get; set; }

        [Required]
        [Editable(true)]
        public bool Enabled { get; set; }

        [Editable(true)]
        public string IpAddress { get; set; }

        [Required]
        [Editable(true)]
        public DateTime LastRecordedDate { get; set; }

        [Required]
        [Editable(true)]
        public DateTime LastReceivedDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Required]
        [Editable(true)]
        public string PhoneNumber { get; set; }
    }
}
