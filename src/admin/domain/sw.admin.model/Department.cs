using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.admin.model
{
    public class Department : EntityBase<long>, IAggregateRoot
    {
        public Department()
        {
            Active = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, Department modifiedDepartment)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTime.UtcNow;

            Name = modifiedDepartment.Name;
            Notes = modifiedDepartment.Notes;
            CodeErp = modifiedDepartment.CodeErp;
            CompanyId = modifiedDepartment.CompanyId;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            DeletedReason = deletedReason;
        }

        public string Name { get; set; }

        public string Notes { get; set; }

        public string CodeErp { get; set; }

        public long CompanyId { get; set; }

        public virtual ICollection<DepartmentPersonRole> DepartmentPersonRoles { get; set; }

        public virtual Company Company { get; set; }

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
