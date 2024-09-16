using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.LandmarkCategories
{
    public class DeleteLandmarkCategoryResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
