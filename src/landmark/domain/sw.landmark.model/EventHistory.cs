using sw.infrastructure.Domain;
using System;
using System.Text.Json;

namespace sw.landmark.model
{
    public class EventHistory : EntityBase<long>, IAggregateRoot
    {
        public EventHistory()
        {
            Active = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, EventHistory modifiedEventHistory)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTime.UtcNow;

            SensorId = modifiedEventHistory.SensorId;
            Recorded = modifiedEventHistory.Recorded;
            Received = modifiedEventHistory.Received;
            EventValue = modifiedEventHistory.EventValue;
            EventValueJson = modifiedEventHistory.EventValueJson;
            EventPositionId = modifiedEventHistory.EventPositionId;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            DeletedReason = deletedReason;
        }

        public long SensorId { get; set; }
        public DateTimeOffset Recorded { get; set; }
        public DateTimeOffset Received { get; set; }
        public double EventValue { get; set; }
        public JsonDocument EventValueJson { get; set; }
        public long EventPositionId { get; set; }

        public virtual EventPosition EventPosition { get; set; }

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
