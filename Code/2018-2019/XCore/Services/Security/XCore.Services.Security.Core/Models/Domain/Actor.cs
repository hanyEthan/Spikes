using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Relations;

namespace XCore.Services.Security.Core.Models.Domain
{
   public class Actor : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public IList<ActorRole> Roles { get; set; }
        public IList<ActorPrivilege> Privileges { get; set; }
        public IList<ActorClaim> Claims { get; set; }


        public int AppId { get; set; }
        public App App { get; set; }

        #endregion
        #region cst.

        public Actor()
        {
            this.Roles = new List<ActorRole>();
            this.Privileges = new List<ActorPrivilege>();
            this.Claims = new List<ActorClaim>();

        }

        #endregion
    }
}
