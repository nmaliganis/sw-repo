using System;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.LandmarkCategories
{
    public class UpdateLandmarkCategoryResourceParameters
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string CodeErp { get; set; }

        public string Params { get; set; }
    }
}
