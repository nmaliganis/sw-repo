using sw.auth.model.Roles;
using sw.infrastructure.Domain;
using System;

namespace sw.auth.model.Departments;

public class DepartmentRole : EntityBase<long>
{
    public DepartmentRole()
    {
        OnCreated();
    }

    private void OnCreated()
    {
        this.Active = true;
        this.CreatedDate = DateTime.UtcNow;
        this.ModifiedDate = new DateTime(2000, 01, 01);
        this.ModifiedBy = 0;
        this.DeletedDate = new DateTime(2000, 01, 01);
        this.DeletedBy = 0;
        this.DeletedReason = string.Empty;

        this.Active = true;
    }

    public virtual Department Department { get; set; }
    public virtual Role Role { get; set; }

    public virtual bool Active { get; set; }

    #region Audit --> Attributes

    public virtual DateTime CreatedDate { get; set; }
    public virtual long CreatedBy { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual long ModifiedBy { get; set; }
    public virtual DateTime DeletedDate { get; set; }
    public virtual long DeletedBy { get; set; }
    public virtual string DeletedReason { get; set; }

    #endregion

    protected override void Validate()
    {
    }

    public virtual void InjectWithRole(Role roleToBeInjected)
    {
        this.Role = roleToBeInjected;
        roleToBeInjected.Departments.Add(this);
    }

    public virtual void InjectWithAuditCreation(long accountIdToCreateThisUser)
    {
        this.CreatedBy = accountIdToCreateThisUser;
    }
}// Class : DepartmentRole