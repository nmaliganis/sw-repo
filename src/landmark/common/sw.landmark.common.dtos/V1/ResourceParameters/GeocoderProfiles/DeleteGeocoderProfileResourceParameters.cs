using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.GeocoderProfiles
{
    public class DeleteGeocoderProfileResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
