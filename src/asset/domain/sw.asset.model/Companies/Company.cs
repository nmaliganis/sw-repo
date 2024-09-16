using sw.asset.model.Assets;
using sw.asset.model.Persons;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;
using sw.asset.model.Companies.Zones;

namespace sw.asset.model.Companies;

public class Company : EntityBase<long>, IAggregateRoot
{
    public Company()
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

        this.Assets = new HashSet<Asset>();

        this.Zones = new HashSet<Zone>();
    }

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual string CodeErp { get; set; }

    private ISet<Asset> _assets;
    public virtual ISet<Asset> Assets
    {
        get => _assets ?? (_assets = new HashSet<Asset>());
        set => _assets = value;
    }
    public virtual ISet<Department> Departments { get; set; }
    public virtual ISet<Zone> Zones { get; set; }

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

    public virtual void Modified(long modifiedBy, string name, string codeErp, string description)
    {
        Name = name;
        CodeErp = codeErp;
        Description = description;

        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public virtual void Deleted(long deletedBy, string deletedReason)
    {
        Active = false;
        DeletedBy = deletedBy;
        DeletedDate = DateTime.UtcNow;
        DeletedReason = deletedReason;
    }

    public virtual void InjectWithAudit(long accountIdToCreateThisCompany)
    {
        this.CreatedBy = accountIdToCreateThisCompany;
        this.CreatedDate = DateTime.Now;
    }

    public virtual void InjectWithInitialAttributes(string name, string description, string codeErp)
    {
        this.Name = !string.IsNullOrEmpty(name) ? name : string.Empty;
        this.Description = !string.IsNullOrEmpty(description) ? description : string.Empty;
        this.CodeErp = !string.IsNullOrEmpty(codeErp) ? codeErp : string.Empty;
    }

    protected override void Validate()
    {
    }

}// Class: Company