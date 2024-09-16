using System.ComponentModel.DataAnnotations;

namespace sw.asset.model.Geofence;

public class Municipality
{
    [Required]
    [Editable(true)]
    public long Id { get; set; }

    [Required]
    [Editable(true)]
    public string Name { get; set; }
}// Class: Municipality