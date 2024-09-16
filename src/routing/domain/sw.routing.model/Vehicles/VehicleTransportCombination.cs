using System;
using System.Collections.Generic;
using sw.routing.model.TransportCombinations;
using sw.infrastructure.Domain;

namespace sw.routing.model.Vehicles;

public class VehicleTransportCombination : EntityBase<long>
{
    public VehicleTransportCombination()
    {
        OnCreated();
    }

    private void OnCreated()
    {
        Active = true;
        CreatedDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = 0;
        DeletedDate = DateTime.UtcNow;
        DeletedBy = 0;
        DeletedReason = "No Reason";
    }

    public virtual TransportCombinationType Type { get; set; }
    public virtual Vehicle Vehicle { get; set; }

    public virtual TransportCombination TransportCombination { get; set; }


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

    public virtual void InjectWithVehicle(Vehicle vehicleToBeInjected)
    {
        this.Vehicle = vehicleToBeInjected;
        vehicleToBeInjected.TransportCombinations.Add(this);
    }
}