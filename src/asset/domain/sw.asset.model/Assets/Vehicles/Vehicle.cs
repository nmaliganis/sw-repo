using sw.asset.model.Sensors;
using sw.infrastructure.Extensions;
using System;
using System.Collections.Generic;

namespace sw.asset.model.Assets.Vehicles;

public class Vehicle : Asset
{
    public Vehicle()
    {
        this.OnCreated();
    }

    private void OnCreated()
    {
        this.Active = true;
        this.CreatedDate = DateTime.UtcNow;
        this.ModifiedDate = DateTime.UtcNow;
        this.ModifiedBy = 0;
        this.DeletedDate = DateTime.UtcNow;
        this.DeletedBy = 0;
        this.DeletedReason = "No Reason";
        this.Sensors = new List<Sensor>();
    }

    public virtual string NumPlate { get; set; }

    public virtual string Brand { get; set; }

    public virtual DateTime RegisteredDate { get; set; }

    public virtual VehicleType Type { get; set; }

    public virtual VehicleStatus Status { get; set; }

    public virtual GasType Gas { get; set; }

    public virtual double Height { get; set; }

    public virtual double Width { get; set; }

    public virtual double Axels { get; set; }

    public virtual double MinTurnRadius { get; set; }

    public virtual double Length { get; set; }

    public virtual void Created(long createdBy)
    {
        CreatedBy = createdBy;
    }

    public virtual void Modified(long modifiedBy, Vehicle modifiedContainer)
    {
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        // Asset
        Name = modifiedContainer.Name;
        CodeErp = modifiedContainer.CodeErp;
        Image = modifiedContainer.Image;
        Description = modifiedContainer.Description;

        // Container
        NumPlate = modifiedContainer.NumPlate;
        Brand = modifiedContainer.Brand;
        RegisteredDate = modifiedContainer.RegisteredDate;
        Type = modifiedContainer.Type;
        Status = modifiedContainer.Status;
        Gas = modifiedContainer.Gas;
        Height = modifiedContainer.Height;
        Width = modifiedContainer.Width;
        Axels = modifiedContainer.Axels;
        MinTurnRadius = modifiedContainer.MinTurnRadius;
        Length = modifiedContainer.Length;
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
    public override void InjectWithInitialAttributes(string name, string description, string codeErp, string image)
    {
        this.Name = !string.IsNullOrEmpty(name) ? name : string.Empty;
        this.Description = !string.IsNullOrEmpty(description) ? description : string.Empty;
        this.CodeErp = !string.IsNullOrEmpty(codeErp) ? codeErp : string.Empty;
        this.Image = !string.IsNullOrEmpty(image) ? image : string.Empty;
    }
}

// Class: Vehicle