using System;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.GeocodedPositions
{
    public class CreateGeocodedPositionResourceParameters
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [StringLength(300)]
        public string Street { get; set; }

        [StringLength(100)]
        public string Number { get; set; }

        [StringLength(100)]
        public string CrossStreet { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Prefecture { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string Zipcode { get; set; }

        [Required]
        public long GeocoderProfileId { get; set; }

        [Required]
        public DateTimeOffset LastGeocoded { get; set; }
    }
}
