using sw.asset.model.Assets;
using sw.asset.model.Devices;
using sw.asset.model.Events;
using sw.asset.model.SensorTypes;
using sw.infrastructure.CustomTypes;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.asset.model.Sensors;
public class Sensor : EntityBase<long>, IAggregateRoot
{
    public Sensor()
    {
        this.OnCreated();
    }

    private void OnCreated()
    {
        this.Active = true;
        this.IsVisible = true;
        this.Order = 1;
        this.MinValue = 0;
        this.MaxValue = 0;
        this.MinNotifyValue = 0;
        this.MaxNotifyValue = 0;
        this.LastValue = 0;
        this.HighThreshold = 0;
        this.LowThreshold = 0;
        this.SamplingInterval = 0;
        this.ReportingInterval = 0;

        this.CreatedDate = DateTime.UtcNow.ToUniversalTime();
        this.ModifiedDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.ModifiedBy = 0;
        this.DeletedDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.DeletedBy = 0;
        this.DeletedReason = "No Reason";
        this.LastRecordedDate = DateTime.UtcNow.ToUniversalTime();
        this.LastReceivedDate = DateTime.UtcNow.ToUniversalTime();
        this.Params = new JsonType()
        {
            Params = Guid.NewGuid().ToString()
        };

        this.Name = Guid.NewGuid().ToString();
        this.CodeErp = this.Name;

        this.Events = new HashSet<EventHistory>();
    }

    public virtual JsonType Params { get; set; }
    public virtual string Name { get; set; }
    public virtual string CodeErp { get; set; }
    public virtual bool IsActive { get; set; }
    public virtual bool IsVisible { get; set; }
    public virtual long Order { get; set; }
    public virtual double MinValue { get; set; }
    public virtual double MaxValue { get; set; }
    public virtual double MinNotifyValue { get; set; }
    public virtual double MaxNotifyValue { get; set; }
    public virtual double LastValue { get; set; }
    public virtual DateTime LastRecordedDate { get; set; }
    public virtual DateTime LastReceivedDate { get; set; }
    public virtual double HighThreshold { get; set; }
    public virtual double LowThreshold { get; set; }
    public virtual long SamplingInterval { get; set; }
    public virtual long ReportingInterval { get; set; }

    public virtual SensorType SensorType { get; set; }
    public virtual Device Device { get; set; }
    public virtual Asset Asset { get; set; }
    public virtual ISet<EventHistory> Events { get; set; }

    #region Audit --> Attributes
    public virtual bool Active { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual long CreatedBy { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual long ModifiedBy { get; set; }

    public virtual DateTime DeletedDate { get; set; }
    public virtual long DeletedBy { get; set; }
    public virtual string DeletedReason { get; set; }

    #endregion

    public virtual void Created(long createdBy)
    {
        this.CreatedBy = createdBy;
    }

    public virtual void Modified(long modifiedBy, Sensor modifiedSensor)
    {
        this.ModifiedBy = modifiedBy;
        this.ModifiedDate = DateTime.UtcNow;

        this.Params = modifiedSensor.Params;
        this.Name = modifiedSensor.Name;
        this.CodeErp = modifiedSensor.CodeErp;
        this.IsActive = modifiedSensor.IsActive;
        this.IsVisible = modifiedSensor.IsVisible;
        this.Order = modifiedSensor.Order;
        this.MinValue = modifiedSensor.MinValue;
        this.MaxValue = modifiedSensor.MaxValue;
        this.MinNotifyValue = modifiedSensor.MinNotifyValue;
        this.MaxNotifyValue = modifiedSensor.MaxNotifyValue;
        this.LastValue = modifiedSensor.LastValue;
        this.LastRecordedDate = modifiedSensor.LastRecordedDate;
        this.LastReceivedDate = modifiedSensor.LastReceivedDate;
        this.HighThreshold = modifiedSensor.HighThreshold;
        this.LowThreshold = modifiedSensor.LowThreshold;
        this.SamplingInterval = modifiedSensor.SamplingInterval;
        this.ReportingInterval = modifiedSensor.ReportingInterval;
    }

    public virtual void Deleted(long deletedBy, string deletedReason)
    {
        this.Active = false;
        this.DeletedBy = deletedBy;
        this.DeletedDate = DateTime.UtcNow;
        this.DeletedReason = deletedReason;
    }

    protected override void Validate()
    {
    }

    public virtual void InjectWithAudit(long createCommandCreatedById)
    {
        this.CreatedBy = createCommandCreatedById;
        this.CreatedDate = DateTime.UtcNow;
    }

    public virtual void InjectWithInitialAttributes(string name, string paramsStr, string codeErp, bool isActive, DateTime lastReceivedDate, DateTime lastRecordedDate)
    {
        this.Name = !string.IsNullOrEmpty(name) ? name : string.Empty;
        this.Params = new JsonType()
        {
            Params = !string.IsNullOrEmpty(paramsStr) ? paramsStr : string.Empty 
        };
        this.CodeErp = !string.IsNullOrEmpty(codeErp) ? codeErp : string.Empty;
        this.IsActive = isActive;
        this.LastReceivedDate = lastReceivedDate;
        this.LastRecordedDate = lastRecordedDate;
    }


    public virtual void InjectWithSensorType(SensorType sensorTypeModelToBeInjected)
    {
        this.SensorType = sensorTypeModelToBeInjected;
        sensorTypeModelToBeInjected.Sensors.Add(this);
        this.Name = sensorTypeModelToBeInjected.Name;
    }

    public virtual void InjectWithDevice(Device deviceToBeInjected)
    {
        this.Device = deviceToBeInjected;
        deviceToBeInjected.Sensors.Add(this);
    }

    public virtual void InjectWithAsset(Asset assetToBeInjected)
    {
        this.Asset = assetToBeInjected;
        assetToBeInjected.Sensors.Add(this);
    }

}// Class: Sensor