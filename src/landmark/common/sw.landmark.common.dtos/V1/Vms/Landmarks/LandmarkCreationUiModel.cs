using sw.infrastructure.DTOs.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.Vms.Landmarks
{
    public class LandmarkCreationUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Street { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Number { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CrossStreet { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Prefecture { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Country { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Zipcode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string PhoneNumber2 { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Fax { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Url { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string PersonInCharge { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Vat { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Image { get; set; }

        [Editable(true)]
        public bool IsBase { get; set; }

        [Editable(true)]
        public bool ExcludeFromSpace { get; set; }

        [Editable(true)]
        public bool HasSpacePriority { get; set; }

        [Editable(true)]
        public long SpeedLimit { get; set; }

        [Editable(true)]
        public DateTimeOffset Expired { get; set; }

        [Editable(true)]
        public long RootId { get; set; }

        [Editable(true)]
        public long ParentId { get; set; }

        [Editable(true)]
        public long LandmarkCategoryId { get; set; }
    }
}
