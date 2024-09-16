using System.Collections.Generic;

namespace sw.routing.model.CustomTypes;

public class OccurrenceJsonbType
{
    public OccurrenceJsonbType()
    {
        this.Occurrence = new List<int>();
    }
    public virtual List<int> Occurrence { get; set; }
}