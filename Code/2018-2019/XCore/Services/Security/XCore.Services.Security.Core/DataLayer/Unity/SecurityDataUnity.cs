using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.DataLayer.Context;
using XCore.Services.Security.Core.DataLayer.Contracts;
using XCore.Services.Security.Core.DataLayer.Repositories;

namespace XCore.Services.Security.Core.DataLayer.Unity
{
    public class SecurityDataUnity : ISecurityDataUnity
    {
        #region cst.
        public SecurityDataUnity(SecurityDataContext dataContext, IRoleRepository Roles, IActorRepository Actors,
            ITargetRepository Targets, IPrivilegeRepository Privileges, IAppsRepository Apps, IClaimRepository Claims)
        {
            this._DataContext = dataContext;
            this.Actors = Actors;
            this.Apps = Apps;
            this.Privileges = Privileges;
            this.Roles = Roles;
            this.Targets = Targets;
            this.Claims = Claims;
            this.Initialized = Initialize();

        }
        #endregion
        #region props

        public bool? Initialized { get; protected set; }
        private readonly SecurityDataContext _DataContext;


        public IActorRepository Actors { get; protected set; }

        public IRoleRepository Roles { get; protected set; }

        public ITargetRepository Targets { get; protected set; }

        public IPrivilegeRepository Privileges { get; protected set; }

        public IAppsRepository Apps { get; protected set; }
        public IClaimRepository Claims { get; protected set; }

        #endregion
        #region publics

        public async Task SaveAsync()
        {
            try
            {
                await _DataContext.SaveChangesAsync();
            }
            catch (Exception x)
            {

                throw;
            }
        }
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.Apps?.Initialized.GetValueOrDefault() ?? false)
                && (this.Roles?.Initialized.GetValueOrDefault() ?? false)
                && (this.Actors?.Initialized.GetValueOrDefault() ?? false)
                && (this.Privileges?.Initialized.GetValueOrDefault() ?? false)
                && (this.Targets?.Initialized.GetValueOrDefault() ?? false)
                && (this.Claims?.Initialized.GetValueOrDefault() ?? false);




            return isValid;
        }

        #endregion
    }
}
