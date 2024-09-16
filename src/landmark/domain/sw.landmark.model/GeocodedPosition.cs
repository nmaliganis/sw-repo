using sw.infrastructure.Domain;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace sw.landmark.model
{
    public class GeocodedPosition : EntityBase<long>, IAggregateRoot
    {
        public GeocodedPosition()
        {
            Active = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, GeocodedPosition modifiedGeocodedPosition)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTime.UtcNow;

            Position = modifiedGeocodedPosition.Position;
            Street = modifiedGeocodedPosition.Street;
            Number = modifiedGeocodedPosition.Number;
            CrossStreet = modifiedGeocodedPosition.CrossStreet;
            City = modifiedGeocodedPosition.City;
            Prefecture = modifiedGeocodedPosition.Prefecture;
            Country = modifiedGeocodedPosition.Country;
            Zipcode = modifiedGeocodedPosition.Zipcode;
            GeocoderProfileId = modifiedGeocodedPosition.GeocoderProfileId;
            LastGeocoded = modifiedGeocodedPosition.LastGeocoded;

        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            DeletedReason = deletedReason;
        }

        public Point Position { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string CrossStreet { get; set; }
        public string City { get; set; }
        public string Prefecture { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public long GeocoderProfileId { get; set; }
        public DateTimeOffset LastGeocoded { get; set; }

        public virtual ICollection<EventPosition> EventPositions { get; set; }
        public virtual GeocoderProfile GeocoderProfile { get; set; }

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
