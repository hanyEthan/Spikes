using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Relations;

namespace XCore.Services.Security.Core.Models.Domain
{
    public class Claim : Entity<int>
    {
        #region props.

        public string Key { get; set; }
        public string Value { get; set; }
        public string Map { get; set; }
        public string Description { get; set; }
        //public IList<RoleClaim> Roles { get; set; }
        //public IList<ActorClaim> Actors { get; set; }
        public int AppId { get; set; }
        public App App { get; set; }

        #endregion
        #region cst.

        public Claim()
        {
            //this.Actors = new List<ActorClaim>();
            //this.Roles = new List<RoleClaim>();
        }

        #endregion
    }
}
