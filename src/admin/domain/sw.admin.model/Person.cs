using sw.infrastructure.Domain;
using System;

namespace sw.admin.model
{
    public class Person : EntityBase<long>, IAggregateRoot
    {
        public Person()
        {
            Active = true;
            CreatedDate = DateTimeOffset.UtcNow;
        }

        public void Created(long createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Modified(long modifiedBy, Person modifiedPerson)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = DateTimeOffset.UtcNow;

            FirstName = modifiedPerson.FirstName;
            LastName = modifiedPerson.LastName;
            Gender = modifiedPerson.Gender;
            Phone = modifiedPerson.Phone;
            ExtPhone = modifiedPerson.ExtPhone;
            Notes = modifiedPerson.Notes;
            Email = modifiedPerson.Email;
            AddressStreet1 = modifiedPerson.AddressStreet1;
            AddressStreet2 = modifiedPerson.AddressStreet2;
            AddressPostCode = modifiedPerson.AddressPostCode;
            AddressCity = modifiedPerson.AddressCity;
            AddressRegion = modifiedPerson.AddressRegion;
            Mobile = modifiedPerson.Mobile;
            ExtMobile = modifiedPerson.ExtMobile;
            Status = modifiedPerson.Status;
            PersonRoleId = modifiedPerson.PersonRoleId;
    }

        public void Deleted(long deletedBy, string deletedReason)
        {
            Active = false;
            DeletedBy = deletedBy;
            DeletedDate = DateTimeOffset.UtcNow;
            DeletedReason = deletedReason;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Gender { get; set; }
        public string Phone { get; set; }
        public string ExtPhone { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public string AddressStreet1 { get; set; }
        public string AddressStreet2 { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressRegion { get; set; }
        public string Mobile { get; set; }
        public string ExtMobile { get; set; }
        public long Status { get; set; }
        public long PersonRoleId { get; set; }

        public virtual DepartmentPersonRole DepartmentPersonRole { get; set; }

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
