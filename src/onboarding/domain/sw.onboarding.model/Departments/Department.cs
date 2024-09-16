using System;
using System.Collections.Generic;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.model.Departments;
using sw.auth.model.Members;
using sw.onboarding.model.Companies;
using sw.infrastructure.Domain;

namespace sw.onboarding.model.Departments {
    public class Department : EntityBase<long>, IAggregateRoot {
        public Department() {
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

            this.Members = new HashSet<MemberDepartment>();
            this.Roles = new HashSet<DepartmentRole>();
        }

        public virtual string Name { get; set; }
        public virtual string CodeErp { get; set; }
        public virtual string Notes { get; set; }
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

        #region Dependencies --> Attributes

        public virtual Company Company { get; set; }
        public virtual ISet<MemberDepartment> Members { get; set; }
        public virtual ISet<DepartmentRole> Roles { get; set; }

        #endregion

        protected override void Validate() {
        }

        public virtual void InjectWithInitialAttributes(CreateDepartmentCommand createCommand) {
            this.Name = createCommand.Parameters.Name;
            this.CodeErp = createCommand.Parameters.CodeErp;
            this.Notes = createCommand.Parameters.Notes;
        }

        public virtual void InjectWithAudit(long createCommandCreatedById) {
            this.CreatedBy = createCommandCreatedById;
            this.CreatedDate = DateTime.UtcNow;
        }

        public virtual void InjectWithCompany(Company companyToBeInjected) {
            this.Company = companyToBeInjected;
            companyToBeInjected.Departments.Add(this);
        }

        public virtual void ModifyWithAudit(long registeredUserId, UpdateDepartmentCommand updateCommand)
        {
            this.ModifiedBy = registeredUserId;
            this.ModifiedDate = DateTime.UtcNow;

            this.Name = updateCommand.Parameters.Name;
            this.CodeErp = updateCommand.Parameters.CodeErp;
            this.Notes = updateCommand.Parameters.Notes;
        }

        public virtual void DeleteWithAudit(long registeredUserId, string deletedReason)
        {
            this.Active = false;
            this.DeletedBy = registeredUserId;
            this.DeletedDate = DateTime.UtcNow;
            this.DeletedReason = deletedReason;
        }
    }//Class : Department
}//Namespace : sw.auth.model.Departments