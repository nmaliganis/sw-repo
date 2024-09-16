using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.infrastructure.Domain;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;

namespace sw.routing.model.ItineraryTemplates;

public class ItineraryTemplate : EntityBase<long>, IAggregateRoot
{
    public ItineraryTemplate()
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

        this.Itineraries = new HashSet<Itinerary>();
        this.Locations = new HashSet<ItineraryTemplateLocationPoint>();

        Occurrence = new OccurrenceJsonbType();
        Zones = new ZoneJsonbType();
    }

    public virtual string Name { get; set; }
    public virtual StreamType Stream { get; set; }
    public virtual ZoneJsonbType Zones { get; set; }
    public virtual OccurrenceJsonbType Occurrence { get; set; }
    public virtual string Description { get; set; }
    public virtual double MinFillLevel { get; set; }
    public virtual TimeSpan StartTime { get; set; }

    public virtual ISet<Itinerary> Itineraries { get; set; }
    public virtual ISet<ItineraryTemplateLocationPoint> Locations { get; set; }

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

    public virtual void InjectWithStartFrom(ItineraryTemplateLocationPoint locationPoint)
    {
        this.Locations.Add(locationPoint);
        locationPoint.Template = this;
    }

    public virtual void InjectWithEndTo(ItineraryTemplateLocationPoint locationPoint)
    {
        this.Locations.Add(locationPoint);
        locationPoint.Template = this;
    }

    protected override void Validate()
    {
    }

    public virtual void InjectWithInitialAttributes(CreateItineraryTemplateCommand createCommand)
    {
        this.Name = createCommand.Parameters.Name;
        this.Description = createCommand.Parameters.Description;
        this.Stream = (StreamType)createCommand.Parameters.Stream;
        this.Occurrence.Occurrence = createCommand.Parameters.Occurrence;
        this.MinFillLevel = createCommand.Parameters.MinFillLevel;
        this.StartTime = TimeSpan.Parse(createCommand.Parameters.StartTime);
        this.Zones = new ZoneJsonbType()
        {
            ZoneId = createCommand.Parameters.Zone
        };
    }

    public virtual void InjectWithModifiedAttributes(UpdateItineraryTemplateCommand updateCommand)
    {
        this.Name = updateCommand.Parameters.Name;
        this.Description = updateCommand.Parameters.Description;
        this.Occurrence = new OccurrenceJsonbType
        {
            Occurrence = updateCommand.Parameters.Occurrence
        };

        this.Zones = new ZoneJsonbType
        {
            ZoneId = updateCommand.Parameters.Zones.ZoneId,
            Containers = new List<ContainerItineraryTemplate>()
        };

        foreach (var zonesContainer in updateCommand.Parameters.Zones.Containers)
        {
            this.Zones.Containers.Add(new ContainerItineraryTemplate()
            {
                Id = zonesContainer.Id,
                Level = zonesContainer.Level,
                Latitude = zonesContainer.Latitude,
                Longitude = zonesContainer.Longitude,
                LastServicedDate = zonesContainer.LastServicedDate,
                Status = zonesContainer.Status,
            });
        }
    }

    public virtual void ModifyWithAudit(long updateCommandModifiedById, UpdateItineraryTemplateCommand updateCommand)
    {
        this.ModifiedBy = updateCommandModifiedById;
        this.ModifiedDate = DateTime.UtcNow;
    }
}// Class : ItineraryTemplate