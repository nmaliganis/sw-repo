using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.GeocoderProfiles
{
    public class CreateGeocoderProfileResourceParameters
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string SourceName { get; set; }

        [Required]
        public string Params { get; set; }
    }
}
