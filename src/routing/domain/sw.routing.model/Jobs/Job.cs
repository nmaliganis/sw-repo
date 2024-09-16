using System;
using System.Collections.Generic;
using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.infrastructure.CustomTypes;
using sw.infrastructure.Domain;

namespace sw.routing.model.Jobs;

public class Job : EntityBase<long>, IAggregateRoot
{
    public Job()
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

        this.Children = new HashSet<Job>();
        this.Index = Guid.NewGuid().ToString();
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
    public virtual string Index { get; set; }
    public virtual long Container { get; set; }
    public virtual DateTime Arrival { get; set; }
    public virtual DateTime EstimatedArrival { get; set; }

    public virtual DateTime Departure { get; set; }
    public virtual DateTime EstimatedDeparture { get; set; }
    public virtual DateTime ScheduledArrival { get; set; }

    public virtual ConfigJsonbType Config { get; set; }

    public virtual string Seq { get; set; }


    public virtual Itinerary Itinerary { get; set; }
    public virtual Job Parent { get; set; }
    public virtual ISet<Job> Children { get; set; }

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

}// Class : ItineraryTemplate