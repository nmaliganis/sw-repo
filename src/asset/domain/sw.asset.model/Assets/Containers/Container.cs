using sw.asset.model.Assets.Containers.Types;
using sw.asset.model.Companies.Zones;
using sw.asset.model.Sensors;
using sw.infrastructure.Extensions;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using ContainerStatus = sw.asset.model.Assets.Containers.Types.ContainerStatus;
using ContainerType = sw.asset.model.Assets.Containers.Types.ContainerType;
using MaterialType = sw.asset.model.Assets.Containers.Types.MaterialType;

namespace sw.asset.model.Assets.Containers;

public class Container : Asset
{
    public Container()
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
        this.Sensors = new List<Sensor>();

        this.Capacity = ContainerCapacity.OneThousandAndHundred_1100;
        this.Material = MaterialType.HDPE;

        this.ContainerStatus = ContainerStatus.Normal;
        this.ContainerCondition = ContainerCondition.Empty;

        this.LastServicedDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
    }

    public virtual bool IsVisible { get; set; }
    public virtual int Level { get; set; }
    public virtual int PrevLevel { get; set; }
    public virtual Geometry GeoPoint { get; set; }

    public virtual double TimeToFull { get; set; }
    public virtual DateTime LastServicedDate { get; set; }
    public virtual ContainerStatus ContainerStatus { get; set; }
    public virtual ContainerCondition ContainerCondition { get; set; }
    public virtual DateTime MandatoryPickupDate { get; set; }
    public virtual bool MandatoryPickupActive { get; set; }
    public virtual ContainerCapacity Capacity { get; set; }
    public virtual ContainerType WasteType { get; set; }
    public virtual MaterialType Material { get; set; }
    public virtual Zone Zone { get; set; }

    public virtual void Modified(long modifiedBy, Container modifiedContainer)
    {
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        // Asset
        Name = modifiedContainer.Name;
        CodeErp = modifiedContainer.CodeErp;
        Image = modifiedContainer.Image;
        Description = modifiedContainer.Description;

        // Container
        IsVisible = modifiedContainer.IsVisible;
        PrevLevel = this.Level;
        Level = modifiedContainer.Level;
        GeoPoint = modifiedContainer.GeoPoint;
        TimeToFull = modifiedContainer.TimeToFull;
        LastServicedDate = modifiedContainer.LastServicedDate;
        ContainerStatus = modifiedContainer.ContainerStatus;
        MandatoryPickupDate = modifiedContainer.MandatoryPickupDate;
        MandatoryPickupActive = modifiedContainer.MandatoryPickupActive;
        Capacity = modifiedContainer.Capacity;
        WasteType = modifiedContainer.WasteType;
        Material = modifiedContainer.Material;
    }

    public virtual void Deleted(long deletedBy, string deletedReason)
    {
        Active = false;
        DeletedBy = deletedBy;
        DeletedDate = DateTime.UtcNow;
        DeletedReason = deletedReason;
    }
    
    public virtual void InjectWithZone(Zone zoneToBeInjected)
    {
        this.Zone = zoneToBeInjected;
        zoneToBeInjected.Containers.Add(this);
    }

    public virtual void CalculateLevel(double range)
    {
        int height = 930;
        if (this.Material == MaterialType.HDPE)
        {
            if (this.Capacity == ContainerCapacity.ΟneΗundredΑndΤwenty_120)
            {
                height = 800;
            }
            else if (this.Capacity == ContainerCapacity.TwoHundredAndForty_240)
            {
                height = 885;
            }
            else if (this.Capacity == ContainerCapacity.SixHundredAndSixty_660)
            {
                height = 900;
            }
            else if (this.Capacity == ContainerCapacity.OneThousandAndHundred_1100)
            {
                height = 930;
            }
        }
        else if (this.Material == MaterialType.Metallic)
        {
            if (Capacity == ContainerCapacity.OneThousandAndHundred_1100)
            {
                height = 1000;
            }
        }

        this.Level = Math.Min((int)Math.Round((Math.Abs(height - range) / height) * 100), 100);
        if (this.Level < 10)
            this.ContainerCondition = ContainerCondition.Empty;
        else if (this.Level > 85)
            this.ContainerCondition = ContainerCondition.Full;
        else
            this.ContainerCondition = ContainerCondition.Normal;
    }

    public virtual void InjectWithInitialContainerAttributes(int material, int capacity, int type)
    {
        this.Material = (MaterialType)material;
        this.Capacity = (ContainerCapacity)capacity;
        this.WasteType = (ContainerType)type;
    }
    
    public virtual void InjectWithMandatoryPickupContainerAttributes(bool mandatoryPickupActive, DateTime mandatoryPickupDate)
    {
        this.MandatoryPickupActive = mandatoryPickupActive;
        this.MandatoryPickupDate = mandatoryPickupDate;
    }

    protected override void Validate()
    {
    }

    public override void InjectWithInitialAttributes(string name, string description, string codeErp, string image)
    {
        this.Name = !string.IsNullOrEmpty(name) ? name : string.Empty;
        this.Description = !string.IsNullOrEmpty(description) ? description : string.Empty;
        this.CodeErp = !string.IsNullOrEmpty(codeErp) ? codeErp : string.Empty;
        this.Image = !string.IsNullOrEmpty(image) ? image : string.Empty;
    }
}// Class: Container