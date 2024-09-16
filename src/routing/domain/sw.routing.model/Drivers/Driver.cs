using System;
using System.Collections.Generic;
using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.Swifts;
using sw.routing.model.TransportCombinations;
using sw.infrastructure.Domain;

namespace sw.routing.model.Drivers;

public class Driver : EntityBase<long>, IAggregateRoot
{
    public Driver()
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

        this.Swifts = new HashSet<Swift>();

        this.TransportCombinations = new HashSet<DriverTransportCombination>();
    }
    
    public virtual string Firstname { get; set; }
    public virtual string Lastname { get; set; }
    public virtual string Email { get; set; }
    public virtual long Member { get; set; }
    public virtual DriverGenderType Gender { get; set; }
    
    public virtual ISet<DriverTransportCombination> TransportCombinations { get; set; }

    public virtual ISet<Swift> Swifts{ get; set; }

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

}// Class : Driver