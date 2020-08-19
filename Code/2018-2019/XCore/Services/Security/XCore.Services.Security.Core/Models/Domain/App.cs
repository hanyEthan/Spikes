using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Security.Core.Models.Domain
{
   public class App : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public IList<Privilege> Privileges { get; set; }
        public IList<Role> Roles { get; set; }
        public IList<Actor> Actors { get; set; }
        public IList<Target> Targets { get; set; }

        #endregion
        #region cst.

        public App()
        {
            this.Roles = new List<Role>();
            this.Privileges = new List<Privilege>();
            this.Targets = new List<Target>();
            this.Actors = new List<Actor>();
        }

        #endregion
    }
}
