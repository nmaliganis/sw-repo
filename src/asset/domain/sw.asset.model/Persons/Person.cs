using System;
using System.Collections.Generic;
using sw.asset.model.Assets;
using sw.infrastructure.Domain;

namespace sw.asset.model.Persons;

public class Person : EntityBase<long>, IAggregateRoot
{
  public Person()
  {
    this.OnCreated();
  }

  private void OnCreated()
  {
    this.Active = true;
    this.CreatedDate = DateTime.UtcNow;
    this.ModifiedDate = DateTime.UtcNow;
    this.ModifiedBy = 0;
    this.DeletedDate =  DateTime.UtcNow;
    this.DeletedBy = 0;
    this.DeletedReason = "No Reason";

    this.Departments = new HashSet<DepartmentPerson>();
  }

  public virtual string FirstName { get; set; }
  public virtual string LastName { get; set; }
  public virtual string Email { get; set; }
  public virtual string Username { get; set; }

  public virtual ISet<DepartmentPerson> Departments { get; set; }

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


  protected override void Validate()
  {
  }

}// Class: Company