using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.EventPositions
{
    public class DeleteEventPositionResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
