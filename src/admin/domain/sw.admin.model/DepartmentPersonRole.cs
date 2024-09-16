using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.admin.model
{
    public class DepartmentPersonRole : EntityBase<long>, IAggregateRoot
    {
        public DepartmentPersonRole()
        {
            Active = true;
            CreatedDate = DateTimeOffset.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, DepartmentPersonRole modifiedDepartmentPersonRole)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTimeOffset.UtcNow;

            Name = modifiedDepartmentPersonRole.Name;
            Notes = modifiedDepartmentPersonRole.Notes;
            CodeErp = modifiedDepartmentPersonRole.CodeErp;
            DepartmentId = modifiedDepartmentPersonRole.DepartmentId;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTimeOffset.UtcNow;
            DeletedReason = deletedReason;
        }

        public string Name { get; set; }

        public string Notes { get; set; }

        public string CodeErp { get; set; }

        public long DepartmentId { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

        public virtual Department Department { get; set; }

        protected override void Validate()
        {
        }

        // TODO: Later move to EntityBase
        public bool Active { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTimeOffset DeletedDate { get; set; }
        public long DeletedBy { get; set; }
        public string DeletedReason { get; set; }
    }
}
