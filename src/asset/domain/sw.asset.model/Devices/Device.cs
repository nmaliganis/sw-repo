using sw.asset.model.Devices.Simcards;
using sw.asset.model.Sensors;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Net;

namespace sw.asset.model.Devices;

public class Device : EntityBase<long>, IAggregateRoot
{
    public Device()
    {
        this.OnCreated();
    }

    private void OnCreated()
    {
        this.Active = true;
        this.CreatedDate = DateTime.UtcNow.ToUniversalTime();
        this.ModifiedDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.ModifiedBy = 0;
        this.DeletedDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.DeletedBy = 0;
        this.DeletedReason = "No Reason";

        this.ActivationCode = Guid.NewGuid().ToString();
        this.ActivationDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.ProvisioningCode = Guid.NewGuid().ToString();
        this.ProvisioningDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.ResetCode = Guid.NewGuid().ToString();
        this.ResetDate = DateTime.UtcNow;

        this.IpAddress = IPAddress.Parse("10.0.0.1");

        this.Activated = true;
        this.Enabled = true;

        this.Sensors = new HashSet<Sensor>();
    }

    public virtual string Imei { get; set; }
    public virtual string SerialNumber { get; set; }
    public virtual string ActivationCode { get; set; }
    public virtual DateTime ActivationDate { get; set; }
    public virtual long ActivationBy { get; set; }
    public virtual string ProvisioningCode { get; set; }
    public virtual long ProvisioningBy { get; set; }
    public virtual DateTime ProvisioningDate { get; set; }
    public virtual string ResetCode { get; set; }
    public virtual long ResetBy { get; set; }
    public virtual DateTime ResetDate { get; set; }
    public virtual bool Activated { get; set; }
    public virtual bool Enabled { get; set; }
    public virtual IPAddress IpAddress { get; set; }
    public virtual DateTime LastRecordedDate { get; set; }
    public virtual DateTime LastReceivedDate { get; set; }
    public virtual string CodeErp { get; set; }
    public virtual string Notes { get; set; }

    public virtual ISet<Sensor> Sensors { get; set; }
    public virtual DeviceModel DeviceModel { get; set; }
    public virtual Simcard Simcard { get; set; }

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

    public virtual void Modified(long modifiedBy, Device modifiedDevice)
    {
        this.ModifiedBy = modifiedBy;
        this.ModifiedDate = DateTime.UtcNow;

        this.Imei = modifiedDevice.Imei;
        this.SerialNumber = modifiedDevice.SerialNumber;
        this.ActivationCode = modifiedDevice.ActivationCode;
        this.ActivationDate = modifiedDevice.ActivationDate;
        this.ActivationBy = modifiedDevice.ActivationBy;
        this.ProvisioningCode = modifiedDevice.ProvisioningCode;
        this.ProvisioningBy = modifiedDevice.ProvisioningBy;
        this.ProvisioningDate = modifiedDevice.ProvisioningDate;
        this.ResetCode = modifiedDevice.ResetCode;
        this.ResetBy = modifiedDevice.ResetBy;
        this.ResetDate = modifiedDevice.ResetDate;
        this.Activated = modifiedDevice.Activated;
        this.Enabled = modifiedDevice.Enabled;
        this.IpAddress = modifiedDevice.IpAddress;
        this.LastRecordedDate = modifiedDevice.LastRecordedDate;
        this.LastReceivedDate = modifiedDevice.LastReceivedDate;
        this.CodeErp = modifiedDevice.CodeErp;
        this.Notes = modifiedDevice.Notes;
    }

    public virtual void InjectWithAudit(long createCommandCreatedById)
    {
        this.CreatedBy = createCommandCreatedById;
        this.CreatedDate = DateTime.UtcNow;
    }

    public virtual void InjectWithInitialAttributes(string imei, string serialNumber, string ipAddress)
    {
        this.Imei = !string.IsNullOrEmpty(imei) ? imei : string.Empty;
        this.SerialNumber = !string.IsNullOrEmpty(serialNumber) ? serialNumber : string.Empty;
        this.IpAddress = IPAddress.Parse(!string.IsNullOrEmpty(ipAddress) ? ipAddress : "10.0.0.1");
    }

    public virtual void Deleted(long deletedBy, string deletedReason)
    {
        this.Active = false;
        this.DeletedBy = deletedBy;
        this.DeletedDate = DateTime.UtcNow;
        this.DeletedReason = deletedReason;
    }

    public virtual void InjectWithDeviceModel(DeviceModel deviceModelToBeInjected)
    {
        this.DeviceModel = deviceModelToBeInjected;
        deviceModelToBeInjected.Devices.Add(this);
    }

    public virtual void InjectWithSimcard(Simcard simcardToBeInjected)
    {
        this.Simcard = simcardToBeInjected;
        simcardToBeInjected.Device = this;
    }

    protected override void Validate()
    {
    }
}// Class: Device
