using sw.infrastructure.Domain;
using System;

namespace sw.landmark.model
{
    public class Landmark : EntityBase<long>, IAggregateRoot
    {
        public Landmark()
        {
            Active = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, Landmark modified)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTime.UtcNow;

            Name = modified.Name;
            Description = modified.Description;
            CodeErp = modified.CodeErp;
            Street = modified.Street;
            Number = modified.Number;
            CrossStreet = modified.CrossStreet;
            City = modified.City;
            Prefecture = modified.Prefecture;
            Country = modified.Country;
            Zipcode = modified.Zipcode;
            PhoneNumber = modified.PhoneNumber;
            PhoneNumber2 = modified.PhoneNumber2;
            Email = modified.Email;
            Fax = modified.Fax;
            Url = modified.Url;
            PersonInCharge = modified.PersonInCharge;
            Vat = modified.Vat;
            Image = modified.Image;
            IsBase = modified.IsBase;
            ExcludeFromSpace = modified.ExcludeFromSpace;
            HasSpacePriority = modified.HasSpacePriority;
            SpeedLimit = modified.SpeedLimit;
            Expired = modified.Expired;
            RootId = modified.RootId;
            ParentId = modified.ParentId;
            LandmarkCategoryId = modified.LandmarkCategoryId;
        }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            DeletedReason = deletedReason;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CodeErp { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string CrossStreet { get; set; }
        public string City { get; set; }
        public string Prefecture { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Url { get; set; }
        public string PersonInCharge { get; set; }
        public string Vat { get; set; }
        public string Image { get; set; }
        public bool IsBase { get; set; }
        public bool ExcludeFromSpace { get; set; }
        public bool HasSpacePriority { get; set; }
        public long SpeedLimit { get; set; }
        public DateTimeOffset Expired { get; set; }
        public long RootId { get; set; }
        public long ParentId { get; set; }
        public long LandmarkCategoryId { get; set; }

        public virtual LandmarkCategory LandmarkCategory { get; set; }
        public virtual Landmark Root { get; set; }
        public virtual Landmark Parent { get; set; }


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
