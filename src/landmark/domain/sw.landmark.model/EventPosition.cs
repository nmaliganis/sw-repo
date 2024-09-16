using sw.infrastructure.Domain;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace sw.landmark.model
{
    public class EventPosition : EntityBase<long>, IAggregateRoot
    {
        public EventPosition()
        {
            Active = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, EventPosition modifiedEventPosition)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTime.UtcNow;

            Position = modifiedEventPosition.Position;
            GeocodedPositionId = modifiedEventPosition.GeocodedPositionId;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            DeletedReason = deletedReason;
        }

        public Point Position { get; set; }

        public long GeocodedPositionId { get; set; }


        public virtual ICollection<EventHistory> EventHistories { get; set; }
        public virtual GeocodedPosition GeocodedPosition { get; set; }

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
