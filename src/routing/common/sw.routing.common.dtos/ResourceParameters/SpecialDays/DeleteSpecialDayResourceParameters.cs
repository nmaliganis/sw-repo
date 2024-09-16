using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.SpecialDays
{
    public class DeleteSpecialDayResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
