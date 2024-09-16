using System;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.Vms.ItineraryTemplates
{
    public class ItineraryTemplateCreationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Message { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }
        [Required]
        [Editable(true)]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Editable(true)]
        public DateTime ModifiedDate { get; set; }
        [Required]
        [Editable(true)]
        public bool Active { get; set; }
        [Editable(true)]
        public long Id { get; set; }
    }
}