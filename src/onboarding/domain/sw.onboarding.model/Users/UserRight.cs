using sw.auth.model.Roles;
using sw.infrastructure.Domain;
using System.Collections.Generic;

namespace sw.auth.model.Users
{

    public class UserRight : EntityBase<long>, IAggregateRoot
    {
        public UserRight()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.Roles = new HashSet<Role>();
        }

        public virtual ISet<Role> Roles { get; set; }

        protected override void Validate()
        {

        }
    }
}
