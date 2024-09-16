using sw.infrastructure.Domain;
using System;
using System.Collections.Generic;

namespace sw.asset.model.Devices
{
    public class DeviceModel : EntityBase<long>, IAggregateRoot
    {
        public DeviceModel()
        {
            this.OnCreated();
        }

        private void OnCreated()
        {
            this.Active = true;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;
            this.ModifiedBy = 0;
            this.DeletedDate = DateTime.UtcNow;
            this.DeletedBy = 0;
            this.DeletedReason = "No Reason";
            this.Devices = new HashSet<Device>();
        }

        public virtual string Name { get; set; }

        public virtual string CodeName { get; set; }

        public virtual string CodeErp { get; set; }

        public virtual bool Enabled { get; set; }


        public virtual ISet<Device> Devices { get; set; }


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

        public virtual void Created(long createdBy)
        {
            this.CreatedBy = createdBy;
        }

        public virtual void Modified(long modifiedBy, string name, string codeErp, string codeName, bool enabled)
        {
            this.Name = name;
            this.CodeErp = codeErp;
            this.CodeName = codeName;
            this.Enabled = enabled;

            this.ModifiedBy = modifiedBy;
            this.ModifiedDate = DateTime.UtcNow;
        }

        public virtual void Deleted(long deletedBy, string deletedReason)
        {
            this.Active = false;
            this.DeletedBy = deletedBy;
            this.DeletedDate = DateTime.UtcNow;
            this.DeletedReason = deletedReason;
        }



        protected override void Validate()
        {
        }

    }// Class: DeviceModel
}// Namespace: sw.asset.model.Devices
