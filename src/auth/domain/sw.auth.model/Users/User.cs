using sw.auth.model.Members;
using sw.common.dtos.Vms.Accounts;
using sw.infrastructure.Domain;
using sw.infrastructure.Helpers.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using sw.auth.common.dtos.Vms.Accounts;

namespace sw.auth.model.Users {
    public class User : EntityBase<long>, IAggregateRoot {
        public User() {
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

            this.LastLogin = DateTime.UtcNow;
            this.ResetDate = DateTime.UtcNow;

            this.RefreshTokens = new HashSet<RefreshToken>();
        }

        public virtual string Login { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual bool IsActivated { get; set; }
        public virtual DateTime ResetDate { get; set; }
        public virtual Guid ResetKey { get; set; }
        public virtual Guid ActivationKey { get; set; }
        public virtual DateTime LastLogin { get; set; }
        public virtual bool IsLoggedIn { get; set; }
        public virtual bool Disabled { get; set; }

        public virtual bool Active { get; set; }

        public virtual Member Member { get; set; }

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

        public virtual ISet<RefreshToken> RefreshTokens { get; set; }

        #endregion

        protected override void Validate() {

        }

        public virtual void InjectWithInitialAttributes(UserForRegistrationUiModel newUserForRegistration) {
            this.Login = newUserForRegistration.Login;
            this.LastLogin = DateTime.UtcNow;
            this.PasswordHash = HashHelper.Sha512(newUserForRegistration.Password + newUserForRegistration.Login);
            this.IsActivated = false;
            this.ResetDate = new DateTime(2000, 01, 01);
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = new DateTime(2000, 01, 01);
            this.ResetKey = Guid.NewGuid();
            this.ActivationKey = Guid.NewGuid();
        }

        public virtual void InjectWithAuditCreation(long accountIdToCreateThisUser) {
            this.CreatedBy = accountIdToCreateThisUser;
        }

        public virtual void InjectWithMember(Member memberToBeInjected) {
            this.Member = memberToBeInjected;
            memberToBeInjected.User = this;
        }

        public virtual void Activate() {
            this.IsActivated = true;
        }

        public virtual void InjectWithAudit(long accountIdToActivateThisUser) {
            this.ModifiedBy = accountIdToActivateThisUser;
        }

        public virtual void InjectWithRefreshToken(RefreshToken refreshToken) {

            this.RefreshTokens.Add(refreshToken);
            refreshToken.User = this;
        }

        public virtual void ModifyWithRefreshToken(Guid refreshToken) {
            this.RefreshTokens.FirstOrDefault(t => t.JwtToken == refreshToken)!.Expired = true;
        }

        public virtual void ModifyWithAudit(long registeredUserId) {
            this.ModifiedBy = registeredUserId;
            this.ModifiedDate = DateTime.UtcNow;
        }

    }//Class : User

}//Namespace : sw.auth.model.Users
