using sw.infrastructure.Domain;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using sw.routing.common.dtos.Cqrs.Locations;

namespace sw.routing.model.ItineraryTemplates.LocationPoints;

public class LocationPoint : EntityBase<long>, IAggregateRoot
{
    public LocationPoint()
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

        this.Templates = new HashSet<ItineraryTemplateLocationPoint>();
    }

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual Geometry Location { get; set; }


    public virtual ISet<ItineraryTemplateLocationPoint> Templates { get; set; }

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

    public virtual void InjectWithInitialAttributes(CreateLocationCommand createCommand)
    {
        
    }
}// Class : ItineraryTemplate