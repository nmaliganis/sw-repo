using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.GeocodedPositions
{
    public class DeleteGeocodedPositionResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
