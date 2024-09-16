using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Itineraries
{
    public class DeleteItineraryResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
