using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class Event : Entity<int>
    {
        #region props.

        public string Description { get; set; }

        public int  OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual IList<Venue> Venues { get; set; }
        public virtual IList<Department> Departments { get; set; }

        #endregion
        #region cst.

        public Event()
        {
            Venues = new List<Venue>();
            Departments = new List<Department>();
        }
        
        #endregion
    }
}
