using sw.auth.model.Departments;
using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;
using sw.onboarding.common.dtos.Cqrs.Roles;

namespace sw.auth.model.Roles
{
    public class Role : EntityBase<long>, IAggregateRoot
    {
        public Role()
        {
            this.OnCreated();
        }

        private void OnCreated()
        {
            this.Active = true;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = new DateTime(2000, 01, 01).ToUniversalTime();
            this.ModifiedBy = 0;
            this.DeletedDate = new DateTime(2000, 01, 01).ToUniversalTime();
            this.DeletedBy = 0;
            this.DeletedReason = string.Empty;

            this.Departments = new HashSet<DepartmentRole>();
        }

        public virtual string Name { get; set; }
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

        public virtual ISet<DepartmentRole> Departments { get; set; }

        #endregion

        public virtual void InjectWithInitialAttributes(CreateRoleCommand createCommand)
        {
            this.Name = createCommand.Name;
        }

        public virtual void InjectWithAudit(long createCommandCreatedById)
        {
            this.CreatedBy = createCommandCreatedById;
        }

        public virtual void ModifyWithAudit(long modifiedById, UpdateRoleCommand updateCommand)
        {
            this.ModifiedBy = modifiedById;
            this.ModifiedDate = DateTime.UtcNow;

            this.Name = updateCommand.Name;
        }

        public virtual void DeleteWithAudit(long registeredUserId, string deletedReason)
        {
            this.Active = false;
            this.DeletedBy = registeredUserId;
            this.DeletedDate = DateTime.UtcNow;
            this.DeletedReason = deletedReason;
        }

        protected override void Validate()
        {
        }
    }
}