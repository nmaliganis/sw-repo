using System;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.Landmarks
{
    public class UpdateLandmarkResourceParameters
    {
        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        public string CodeErp { get; set; }

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

        [StringLength(100)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string PhoneNumber2 { get; set; }

        [StringLength(300)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Url { get; set; }

        [StringLength(100)]
        public string PersonInCharge { get; set; }

        [StringLength(100)]
        public string Vat { get; set; }

        [StringLength(100)]
        public string Image { get; set; }

        public bool IsBase { get; set; }
        public bool ExcludeFromSpace { get; set; }
        public bool HasSpacePriority { get; set; }
        public long SpeedLimit { get; set; }
        public DateTimeOffset Expired { get; set; }
        public long RootId { get; set; }
        public long ParentId { get; set; }
        public long LandmarkCategoryId { get; set; }
    }
}
