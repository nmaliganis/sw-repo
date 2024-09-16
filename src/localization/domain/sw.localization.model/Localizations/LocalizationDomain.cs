using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.localization.model.Localizations
{
    public class LocalizationDomain : EntityBase<long>, IAggregateRoot
    {
        public LocalizationDomain(long createdBy)
        {
            LocalizationValues = new HashSet<LocalizationValue>();

            Active = true;
            CreatedBy = createdBy;
            CreatedDate = DateTime.UtcNow;
            DeletedReason = string.Empty;
        }

        public void Modified(long modifiedBy)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTime.UtcNow;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            DeletedReason = deletedReason;
        }

        public string Name { get; set; }

        public virtual ICollection<LocalizationValue> LocalizationValues { get; set; }

        protected override void Validate()
        {
        }

        // TODO: Later move to EntityBase
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public long DeletedBy { get; set; }
        public string DeletedReason { get; set; }
    }
}
