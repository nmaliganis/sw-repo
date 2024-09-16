using sw.asset.model.Sensors;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.asset.model.SensorTypes;

public class SensorType : EntityBase<long>, IAggregateRoot
{
    public SensorType()
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

        Sensors = new HashSet<Sensor>();
    }

    public virtual string Name { get; set; }
    public virtual int SensorTypeIndex { get; set; }
    public virtual bool ShowAtStatus { get; set; }
    public virtual long StatusExpiryMinutes { get; set; }
    public virtual bool ShowOnMap { get; set; }
    public virtual bool ShowAtReport { get; set; }
    public virtual bool ShowAtChart { get; set; }
    public virtual bool ResetValues { get; set; }
    public virtual bool SumValues { get; set; }
    public virtual long Precision { get; set; }
    public virtual string Tunit { get; set; }
    public virtual bool CalcPosition { get; set; }
    public virtual string CodeErp { get; set; }

    public virtual ISet<Sensor> Sensors { get; set; }


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
        CreatedBy = createdBy;
    }

    public virtual void Modified(long modifiedBy, SensorType modifiedDevice)
    {
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        Name = modifiedDevice.Name;
        ShowAtStatus = modifiedDevice.ShowAtStatus;
        StatusExpiryMinutes = modifiedDevice.StatusExpiryMinutes;
        ShowOnMap = modifiedDevice.ShowOnMap;
        ShowAtReport = modifiedDevice.ShowAtReport;
        ShowAtChart = modifiedDevice.ShowAtChart;
        ResetValues = modifiedDevice.ResetValues;
        SumValues = modifiedDevice.SumValues;
        Precision = modifiedDevice.Precision;
        Tunit = modifiedDevice.Tunit;
        CalcPosition = modifiedDevice.CalcPosition;
        CodeErp = modifiedDevice.CodeErp;
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


    public virtual void InjectWithAudit(long createCommandCreatedById)
    {
        CreatedBy = createCommandCreatedById;
        CreatedDate = DateTime.Now;
    }
}// Class: SensorType