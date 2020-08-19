using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Relations;

namespace XCore.Services.Security.Core.Models.Domain
{
    public class Role : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public IList<ActorRole> Actors { get; set; }
        public IList<RolePrivilege> Privileges { get; set; }
        public IList<RoleClaim> Claims { get; set; }

        public int AppId { get; set; }
        public App App { get; set; }

        #endregion
        #region cst.

        public Role()
        {
            this.Actors = new List<ActorRole>();
            this.Privileges = new List<RolePrivilege>();
            this.Claims = new List<RoleClaim>();
        }

        #endregion
    }
}
