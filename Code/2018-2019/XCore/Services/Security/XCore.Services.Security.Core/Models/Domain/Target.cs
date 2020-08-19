using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Security.Core.Models.Domain
{
   public class Target : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public int AppId { get; set; }
        public App App { get; set; }

        public int PrivilegeId { get; set; }
        public Privilege Privilege { get; set; }

        #endregion
    }
}
