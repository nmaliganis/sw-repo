using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.model.CustomTypes;
using sw.routing.model.ItineraryTemplates;
using sw.routing.model.Jobs;
using sw.routing.model.TransportCombinations;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.routing.model.Itineraries;

public class Itinerary : EntityBase<long>, IAggregateRoot
{
    public Itinerary()
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
        this.DeletedReason = "No Reason";

        this.Children = new HashSet<Itinerary>();
        this.Jobs = new HashSet<Job>();
    }

    public virtual DriverTransportCombination DriverTransportCombination { get; set; }
    public virtual ItineraryTemplate Template { get; set; }
    public virtual Itinerary Parent { get; set; }
    public virtual ISet<Itinerary> Children { get; set; }
    public virtual ISet<Job> Jobs { get; set; }

    public virtual string Name { get; set; }
    public virtual ConfigJsonbType Config { get; set; }

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

    public virtual void InjectWithInitialAttributes(CreateItineraryCommand createCommand)
    {
        this.Name = createCommand.Parameters.Name;
    }

    public virtual void InjectWithConfig(string config)
    {
        this.Config = new ConfigJsonbType()
        {
            Params = config
        };
    }

    public virtual void InjectWithItineraryTemplate(ItineraryTemplate templateToBeInjected)
    {
        this.Template = templateToBeInjected;
        templateToBeInjected.Itineraries.Add(this);
    }

    public virtual void InjectWithDriverTransportCombination(DriverTransportCombination driverTransportCombination)
    {
        this.DriverTransportCombination = driverTransportCombination;
        driverTransportCombination.Itinerary = this;
    }

    public virtual void InjectWithJob(Job jobToBeInjected)
    {
        this.Jobs.Add(jobToBeInjected);
        jobToBeInjected.Itinerary = this;
    }
}// Class : ItineraryTemplate