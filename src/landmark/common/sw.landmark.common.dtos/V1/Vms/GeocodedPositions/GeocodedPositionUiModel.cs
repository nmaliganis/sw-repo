using sw.infrastructure.DTOs.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.Vms.GeocodedPositions
{
    public class GeocodedPositionUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Editable(true)]
        public double Latitude { get; set; }

        [Editable(true)]
        public double Longitude { get; set; }

        [Editable(true)]
        public string Street { get; set; }

        [Editable(true)]
        public string Number { get; set; }

        [Editable(true)]
        public string CrossStreet { get; set; }

        [Editable(true)]
        public string City { get; set; }

        [Editable(true)]
        public string Prefecture { get; set; }

        [Editable(true)]
        public string Country { get; set; }

        [Editable(true)]
        public string Zipcode { get; set; }

        [Editable(true)]
        public long GeocoderProfileId { get; set; }

        [Editable(true)]
        public DateTimeOffset LastGeocoded { get; set; }
    }
}
