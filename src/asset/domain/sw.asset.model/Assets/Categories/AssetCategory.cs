using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.asset.model.Assets.Categories;

public class AssetCategory : EntityBase<long>, IAggregateRoot
{
    public AssetCategory()
    {
        OnCreated();
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

        this.Assets = new HashSet<Asset>();
    }

    public virtual string Name { get; set; }
    public virtual string CodeErp { get; set; }
    public virtual string Params { get; set; }

    private ISet<Asset> _assets;
    public virtual ISet<Asset> Assets
    {
        get => _assets ?? (_assets = new HashSet<Asset>());
        set => _assets = value;
    }

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

    public virtual void Modified(long modifiedBy, string name, string codeErp, string parameters)
    {
        this.Name = name;
        this.CodeErp = codeErp;
        this.Params = parameters;

        this.ModifiedBy = modifiedBy;
        this.ModifiedDate = DateTime.UtcNow;
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

}// Class: AssetCategory