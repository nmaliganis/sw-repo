using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using sw.infrastructure.CustomTypes;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Sensors
{
    public record SensorUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Editable(true)]
        public long AssetId { get; set; }

        [Editable(true)]
        public long DeviceId { get; set; }

        [Editable(true)]
        public long SensorTypeId { get; set; }

        [Editable(true)]
        public string Params { get; set; }

        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string CodeErp { get; set; }

        [Editable(true)]
        public bool IsActive { get; set; }

        [Editable(true)]
        public bool IsVisible { get; set; }

        [Editable(true)]
        public long Order { get; set; }

        [Editable(true)]
        public double MinValue { get; set; }

        [Editable(true)]
        public double MaxValue { get; set; }

        [Editable(true)]
        public double MinNotifyValue { get; set; }

        [Editable(true)]
        public double MaxNotifyValue { get; set; }

        [Editable(true)]
        public double LastValue { get; set; }

        [Editable(true)]
        public DateTime LastRecordedDate { get; set; }

        [Editable(true)]
        public DateTime LastReceivedDate { get; set; }

        [Editable(true)]
        public double HighThreshold { get; set; }

        [Editable(true)]
        public double LowThreshold { get; set; }

        [Editable(true)]
        public long SamplingInterval { get; set; }

        [Editable(true)]
        public long ReportingInterval { get; set; }
    }
}
