using sw.auth.model.Departments;
using sw.infrastructure.Domain;
using System;
using sw.onboarding.model.Departments;
using sw.onboarding.model.Members;

namespace sw.auth.model.Members {
    public class MemberDepartment : EntityBase<long> {
        public MemberDepartment() {
            this.OnCreated();
        }

        private void OnCreated() {
            this.Active = true;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = new DateTime(2000, 01, 01);
            this.ModifiedBy = 0;
            this.DeletedDate = new DateTime(2000, 01, 01);
            this.DeletedBy = 0;
            this.DeletedReason = string.Empty;

            this.Active = true;
        }

        public virtual Department Department { get; set; }
        public virtual Member Member { get; set; }

        public virtual bool Active { get; set; }

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

        public virtual void InjectWithMember(Member memberToBeInjected) {
            this.Member = memberToBeInjected;
            memberToBeInjected.Departments.Add(this);
        }

        public virtual void InjectWithAuditCreation(long accountIdToCreateThisUser) {
            this.CreatedBy = accountIdToCreateThisUser;
        }

    }// Class : MemberDepartment
}// Namespace : sw.auth.model.Members