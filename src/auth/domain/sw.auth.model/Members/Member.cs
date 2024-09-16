using sw.auth.model.Members.Addresses;
using sw.auth.model.Users;
using sw.common.dtos.Vms.Accounts;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;
using sw.auth.common.dtos.Vms.Accounts;
using sw.auth.model.Departments;
using sw.auth.model.Roles;

namespace sw.auth.model.Members {
    public class Member : EntityBase<long>, IAggregateRoot {
        public Member() {
            this.OnCreated();
        }

        private void OnCreated() {
            this.Active = true;
            this.CreatedDate = DateTime.Now;
            this.ModifiedDate = new DateTime(2000, 01, 01);
            this.ModifiedBy = 0;
            this.DeletedDate = new DateTime(2000, 01, 01);
            this.DeletedBy = 0;
            this.DeletedReason = string.Empty;

        }

        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual MemberGenderType Gender { get; set; }
        public virtual string Phone { get; set; }
        public virtual string ExtPhone { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string ExtMobile { get; set; }
        public virtual string Notes { get; set; }
        public virtual string Email { get; set; }
        public virtual Address Address { get; set; }

        public virtual User User { get; set; }
        public virtual ISet<MemberDepartment> Departments { get; set; }

        #region Audit --> Attributes
        public virtual bool Active { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual long CreatedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual long ModifiedBy { get; set; }

        public virtual DateTime DeletedDate { get; set; }
        public virtual long DeletedBy { get; set; }
        public virtual string DeletedReason { get; set; }

        #endregion

        #region Methods : Overridden
        protected override void Validate() {
        }
        #endregion

        #region Methods : Public

        public virtual void InjectWithAudit(long accountIdToCreateThisMember) {
        }
        public virtual void InjectWithAuditCreation(long accountIdToCreateThisUser) {
            this.CreatedBy = accountIdToCreateThisUser;
        }

        #endregion

        public virtual void InjectWithInitialAttributes(UserForRegistrationUiModel newUserForRegistration) {
            this.Firstname = newUserForRegistration.Firstname;
            this.Lastname = newUserForRegistration.Lastname;
            this.Gender = (MemberGenderType)Enum.Parse(typeof(MemberGenderType), newUserForRegistration.GenderValue, true);
            this.Phone = newUserForRegistration.Phone;
            this.ExtPhone = newUserForRegistration.ExtPhone;
            this.Mobile = newUserForRegistration.Mobile;
            this.ExtMobile = newUserForRegistration.ExtMobile;
            this.Notes = newUserForRegistration.Notes;
            this.Address = new Address() {
                Street = newUserForRegistration.Street,
                StreetNumber = newUserForRegistration.StreetNumber,
                PostCode = newUserForRegistration.AddressPostCode,
                City = newUserForRegistration.AddressCity,
                Region = newUserForRegistration.AddressRegion,
            };
            this.Email = newUserForRegistration.Email;
        }

        public virtual void InjectWithDepartment(MemberDepartment departmentToBeInjected)
        {
            this.Departments.Add(departmentToBeInjected);
            departmentToBeInjected.Member = this;
        }
    }
}