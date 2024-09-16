using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.EventPositions
{
    public class CreateEventPositionResourceParameters
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public long GeocodedPositionId { get; set; }
    }
}
