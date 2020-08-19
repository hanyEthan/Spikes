using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class Department : Entity<int>
    {
        #region Props.

        public virtual string Description { get; set; }

        public int? ParentDepartmentId { get; set; }
        public virtual Department ParentDepartment { get; set; }
        public virtual List<Department> SubDepartments { get; set; }

        public virtual IList<VenueDepartment> Venues { get; set; }
        public virtual IList<DepartmentRole> Roles { get; set; }

        public virtual int? EventId { get; set; }
        public virtual Event Event { get; set; }
        public virtual int OrganizationId { get; set; } = 1;
        public virtual Organization Organization { get; set; }

        #endregion
        #region cst.
        
        public Department()
        {
            SubDepartments = new List<Department>();
            Venues = new List<VenueDepartment>();
            Roles = new List<DepartmentRole>();
        }

        #endregion
    }
}
