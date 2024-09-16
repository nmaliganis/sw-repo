using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Templates
{
    public class DeleteItineraryTemplateResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
