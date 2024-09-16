using System;
using System.Collections.Generic;
using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.model.Departments;
using sw.onboarding.model.Departments;
using sw.infrastructure.Domain;

namespace sw.onboarding.model.Companies;

public class Company : EntityBase<long>, IAggregateRoot
{
  public Company()
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

    this.Departments = new HashSet<Department>();
  }

  public virtual string Name { get; set; }
  public virtual string CodeErp { get; set; }
  public virtual string Description { get; set; }
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

  #region Dependencies --> Attributes

  public virtual ISet<Department> Departments { get; set; }

  #endregion

  protected override void Validate()
  {
  }

  public virtual void InjectWithInitialAttributes(CreateCompanyCommand createCommand)
  {
    this.Name = createCommand.Name;
    this.CodeErp = createCommand.CodeErp;
    this.Description = createCommand.Description;
  }

  public virtual void InjectWithAudit(long createCommandCreatedById)
  {
    this.CreatedBy = createCommandCreatedById;
  }

  public virtual void ModifyWithAudit(long modifiedById, UpdateCompanyCommand updateCommand)
  {
    this.ModifiedBy = modifiedById;
    this.ModifiedDate = DateTime.UtcNow;

    this.Name = updateCommand.Name;
    this.CodeErp = updateCommand.CodeErp;
    this.Description = updateCommand.Description;
  }

  public virtual void DeleteWithAudit(long registeredUserId, string deletedReason)
  {

    this.DeletedBy = registeredUserId;
    this.DeletedDate = DateTime.UtcNow;
    this.DeletedReason = deletedReason;
  }

  public virtual void SoftDeleted()
  {
    this.Active = false;
  }
}//Class : Company
