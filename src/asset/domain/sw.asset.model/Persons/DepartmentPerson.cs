using System;
using sw.infrastructure.Domain;

namespace sw.asset.model.Persons;

public class DepartmentPerson : EntityBase<long>, IAggregateRoot
{
  public DepartmentPerson()
  {
    this.OnCreated();
  }

  private void OnCreated()
  {
    this.Active = true;
    this.CreatedDate = DateTime.UtcNow;
    this.ModifiedDate =  DateTime.UtcNow;
    this.ModifiedBy = 0;
    this.DeletedDate =  DateTime.UtcNow;
    this.DeletedBy = 0;
    this.DeletedReason = "No Reason";
  }

  public virtual string Name { get; set; }
  public virtual Person Person { get; set; }
  public virtual Department Department { get; set; }

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

}// Class: DepartmentPerson
