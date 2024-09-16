using sw.infrastructure.Domain;
using System;

namespace sw.localization.model.Localizations
{
    public class LocalizationValue : EntityBase<long>, IAggregateRoot
    {
        public LocalizationValue(long createdBy)
        {
            Active = true;
            CreatedBy = createdBy;
            CreatedDate = DateTime.UtcNow;
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

        public long LanguageId { get; set; }

        public long DomainId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public virtual LocalizationLanguage LocalizationLanguage { get; set; }

        public virtual LocalizationDomain LocalizationDomain { get; set; }

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
