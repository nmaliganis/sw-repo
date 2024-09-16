using System;
using System.Collections.Generic;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.infrastructure.Domain;
using NetTopologySuite.Geometries;

namespace sw.routing.model.ItineraryTemplates.LocationPoints;

public class ItineraryTemplateLocationPoint : EntityBase<long>
{
    public ItineraryTemplateLocationPoint()
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
    }

    public virtual bool IsStart { get; set; }
    public virtual ItineraryTemplate Template { get; set; }
    public virtual LocationPoint Location { get; set; }


    public virtual ISet<ItineraryTemplate> ItineraryTemplates { get; set; }

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

    public virtual void InjectWithLocation(LocationPoint endToLocation)
    {
        this.Location = endToLocation;
        endToLocation.Templates.Add(this);
    }
}// Class : ItineraryTemplate