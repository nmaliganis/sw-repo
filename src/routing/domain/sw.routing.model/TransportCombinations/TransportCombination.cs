using System;
using System.Collections.Generic;
using sw.routing.model.Drivers;
using sw.routing.model.Vehicles;
using sw.infrastructure.Domain;

namespace sw.routing.model.TransportCombinations;

public class TransportCombination : EntityBase<long>, IAggregateRoot
{
    public TransportCombination()
    {
        this.OnCreated();
    }

    private void OnCreated()
    {
        this.Active = true;
        this.CreatedDate = DateTime.UtcNow;
        this.ModifiedDate = new DateTime(2000, 01, 01).ToUniversalTime();
        this.ModifiedBy = 0;
        this.DeletedDate = new DateTime(2000, 01, 01).ToUniversalTime();
        this.DeletedBy = 0;
        this.DeletedReason = string.Empty;

        this.Vehicles = new HashSet<VehicleTransportCombination>();
        this.DriverItineraries = new HashSet<DriverTransportCombination>();
    }



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

    #region Dependencies --> Attributes
    public virtual ISet<VehicleTransportCombination> Vehicles { get; set; }
    public virtual ISet<DriverTransportCombination> DriverItineraries { get; set; }

    #endregion

    public virtual void InjectWithAudit(long createCommandCreatedById)
    {
        this.CreatedBy = createCommandCreatedById;
    }

    public virtual void DeleteWithAudit(long registeredUserId, string deletedReason)
    {
        this.Active = false;
        this.DeletedBy = registeredUserId;
        this.DeletedDate = DateTime.UtcNow;
        this.DeletedReason = deletedReason;
    }

    protected override void Validate()
    {
    }

    public virtual void InjectWithVehicle(VehicleTransportCombination vehicleTransportCombination)
    {
        this.Vehicles.Add(vehicleTransportCombination);
        vehicleTransportCombination.TransportCombination = this;
    }
}// Class : TransportCombination