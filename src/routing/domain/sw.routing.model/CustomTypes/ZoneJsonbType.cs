using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.model.CustomTypes;

public class ZoneJsonbType
{
    public ZoneJsonbType()
    {
        this.Containers = new List<ContainerItineraryTemplate>();
    }
    public virtual long ZoneId { get; set; }
    public virtual List<ContainerItineraryTemplate> Containers { get; set; }
}

public class ContainerItineraryTemplate
{
    public virtual long Id { get; set; }

    public virtual long Level { get; set; }

    public virtual double Latitude { get; set; }

    public virtual double Longitude { get; set; }

    public virtual DateTime LastServicedDate { get; set; }

    public virtual int Status { get; set; }
}