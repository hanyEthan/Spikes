using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Relations;

namespace XCore.Services.Security.Core.Models.Domain
{
   public class Privilege : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public IList<RolePrivilege> Roles { get; set; }
        public IList<ActorPrivilege> Actors { get; set; }
        public int AppId { get; set; }
        public App App { get; set; }

        //public int TargetId { get; set; }
        public Target Target { get; set; }

        #endregion
        #region cst.

        public Privilege()
        {
            this.Actors = new List<ActorPrivilege>();
            this.Roles = new List<RolePrivilege>();
        }

        #endregion
    }
}
