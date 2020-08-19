using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class Role : Entity<int>
    {
        #region Props.

        public string Description { get; set; }
        public virtual IList<DepartmentRole> Departments { get; set; }

        #endregion
        #region cst.

        public Role()
        {
            Departments = new List<DepartmentRole>();
        }

        #endregion
    }
}
