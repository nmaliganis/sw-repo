using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace sw.landmark.model
{
    public class LandmarkCategory : EntityBase<long>, IAggregateRoot
    {
        public LandmarkCategory()
        {
            Active = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, LandmarkCategory modified)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTime.UtcNow;

            Name = modified.Name;
            Description = modified.Description;
            CodeErp = modified.CodeErp;
            Params = modified.Params;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            DeletedReason = deletedReason;
        }

        public string Name{ get; set; }
        public string Description { get; set; }
        public string CodeErp { get; set; }
        public JsonDocument Params { get; set; }

        public virtual ICollection<Landmark> Landmarks { get; set; }

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
