using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.Vms.SpecialDays
{
    public class SpecialDayDeletionUiModel
    {
        [Required]
        [Editable(true)]
        public long Id { get; set; }
        [Required]
        [Editable(true)]
        public bool Active { get; set; }
        [Required]
        [Editable(true)]
        public bool DeletionStatus { get; set; }
        [Required]
        [Editable(true)]
        public string Message { get; set; }
    }
}