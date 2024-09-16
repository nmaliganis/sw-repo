using sw.infrastructure.Domain;
using System;

namespace sw.auth.model.Users {
    public class RefreshToken : EntityBase<long> {
        public RefreshToken() {

            this.OnCreated();
        }

        private void OnCreated() {
            this.Active = true;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = new DateTime(2000, 01, 01).ToUniversalTime();
            this.ModifiedBy = 0;
            this.DeletedDate = new DateTime(2000, 01, 01).ToUniversalTime();
            this.DeletedBy = 0;
            this.DeletedReason = string.Empty;

            this.Expired = false;
        }
        public virtual Guid JwtToken { get; set; }
        public virtual bool Expired { get; set; }
        public virtual bool Active { get; set; }

        public virtual User User { get; set; }

        #region Audit --> Attributes

        public virtual DateTime CreatedDate { get; set; }
        public virtual long CreatedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual long ModifiedBy { get; set; }
        public virtual DateTime DeletedDate { get; set; }
        public virtual long DeletedBy { get; set; }
        public virtual string DeletedReason { get; set; }

        #endregion

        protected override void Validate() {
        }
    } // Class : RefreshToken
} // Namespace : sw.auth.model.Users
