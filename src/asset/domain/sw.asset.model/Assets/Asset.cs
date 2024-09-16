using sw.asset.model.Assets.Categories;
using sw.asset.model.Companies;
using sw.asset.model.Sensors;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.asset.model.Assets;

public abstract class Asset : EntityBase<long>, IAggregateRoot
{
    public virtual string Name { get; set; }
    public virtual string CodeErp { get; set; }
    public virtual string Image { get; set; }
    public virtual string Description { get; set; }

    public virtual AssetCategory AssetCategory { get; set; }
    public virtual Company Company { get; set; }

    public virtual IList<Sensor> Sensors { get; set; }

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

    public abstract void InjectWithInitialAttributes(string name, string description, string codeErp, string image);

    public virtual void InjectWithAudit(long createCommandCreatedById)
    {
        this.CreatedBy = createCommandCreatedById;
        this.CreatedDate = DateTime.UtcNow;
    }

    public virtual void InjectWithModifiedAudit(long modifiedById)
    {
        this.ModifiedBy = modifiedById;
        this.ModifiedDate = DateTime.UtcNow;
    }
    public virtual void InjectWithCompany(Company companyToBeInjected)
    {
        this.Company = companyToBeInjected;
    }
    public virtual void InjectWithAssetCategory(AssetCategory assetCategoryToBeInjected)
    {
        this.AssetCategory = assetCategoryToBeInjected;
    }

    public virtual void InjectWithSensor(Sensor sensor)
    {
        this.Sensors.Add(sensor);
        sensor.Asset = this;
    }
}// Class: Asset