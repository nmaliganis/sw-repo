using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.EventHistories
{
    public class DeleteEventHistoryResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
