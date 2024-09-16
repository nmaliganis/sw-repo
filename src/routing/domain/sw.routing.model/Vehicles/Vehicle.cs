using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.routing.model.Vehicles;

public class Vehicle : EntityBase<long>, IAggregateRoot
{
    public Vehicle()
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

        this.TransportCombinations = new HashSet<VehicleTransportCombination>();
    }

    public virtual string NumPlate { get; set; }
    public virtual string Brand { get; set; }
    public virtual ISet<VehicleTransportCombination> TransportCombinations { get; set; }

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

        // Vehicle
        NumPlate = modifiedContainer.NumPlate;
        Brand = modifiedContainer.Brand;
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
} // Class: Vehicle