using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.admin.model
{
    public class Company : EntityBase<long>, IAggregateRoot
    {
        public Company()
        {
            Active = true;
            CreatedDate = DateTimeOffset.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, Company modified)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTimeOffset.UtcNow;

            Name = modified.Name;
            CodeErp = modified.CodeErp;
            Description = modified.Description;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTimeOffset.UtcNow;
            DeletedReason = deletedReason;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CodeErp { get; set; }


        public virtual ICollection<Department> Departments { get; set; }

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
