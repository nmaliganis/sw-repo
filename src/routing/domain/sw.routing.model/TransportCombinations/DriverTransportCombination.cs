using sw.routing.model.Drivers;
using sw.routing.model.Itineraries;
using sw.routing.model.Vehicles;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.routing.model.TransportCombinations;

public class DriverTransportCombination : EntityBase<long>
{
    public DriverTransportCombination()
    {
        this.OnCreated();
    }

    private void OnCreated()
    {
        this.Active = true;
        this.CreatedDate = DateTime.UtcNow;
        this.ModifiedDate = DateTime.UtcNow;
        this.ModifiedBy = 0;
        this.DeletedDate = DateTime.UtcNow;
        this.DeletedBy = 0;
        this.DeletedReason = "No Reason";
    }

    public virtual Driver Driver { get; set; }
    public virtual TransportCombination TransportCombination { get; set; }
    public virtual Itinerary Itinerary { get; set; }

    #region Audit --> Attributes

    public virtual DateTime CreatedDate { get; set; }
    public virtual long CreatedBy { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual long ModifiedBy { get; set; }
    public virtual DateTime DeletedDate { get; set; }
    public virtual long DeletedBy { get; set; }
    public virtual string DeletedReason { get; set; }
    public virtual bool Active { get; set; }

    #endregion

    public virtual void Created(long createdBy)
    {
        CreatedBy = createdBy;
    }

    public virtual void Modified(long modifiedBy, Vehicle modifiedContainer)
    {
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public virtual void Deleted(long deletedBy, string deletedReason)
    {
        Active = false;
        DeletedBy = deletedBy;
        DeletedDate = DateTime.UtcNow;
        DeletedReason = deletedReason;
    }

    protected override void Validate()
    {
    }

    public virtual void InjectWithTransportCombination(TransportCombination transportCombination)
    {
        this.TransportCombination = transportCombination;
        transportCombination.DriverItineraries.Add(this);
    }

    public virtual void InjectWithDriver(Driver driverToBeInjected)
    {
        this.Driver = driverToBeInjected;
        driverToBeInjected.TransportCombinations.Add(this);
    }
}