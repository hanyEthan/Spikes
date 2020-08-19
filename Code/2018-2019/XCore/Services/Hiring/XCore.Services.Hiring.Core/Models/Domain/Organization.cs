using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Organization : Entity<int>
    {
        #region props.
        public virtual string AppId { get; set; }
        public virtual string ModuleId { get; set; }
        public virtual string OrganizationReferenceId { get; set; }
        public virtual IList<HiringProcess> HiringProcesses { get; set; }
        public virtual IList<Role> Roles { get; set; }
        
        #endregion
        #region cst.
        public Organization()
        {
            this.HiringProcesses = new List<HiringProcess>();
            this.Roles = new List<Role>();
        } 
        #endregion
    }
}
