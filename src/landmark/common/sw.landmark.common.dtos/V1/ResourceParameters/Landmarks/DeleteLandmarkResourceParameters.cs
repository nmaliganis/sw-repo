using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.Landmarks
{
    public class DeleteLandmarkResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
