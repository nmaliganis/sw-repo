using sw.infrastructure.Domain;

namespace sw.auth.model.Members.Addresses
{
    public class Address : ValueObjectBase
    {
        public virtual string Street { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        public virtual string Region { get; set; }

        protected override void Validate()
        {
        }
    }
}